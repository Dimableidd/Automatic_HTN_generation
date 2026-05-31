import pandas as pd
import numpy as np
from collections import defaultdict, Counter
import json
from tabulate import tabulate

ACTION_MAP = {
    0: 'Idle',
    1: 'GoToTreasure',
    2: 'GoToBase',
    3: 'GoToEnemy',
    4: 'Attack'
}

STATE_FLAGS = [
    "hasTreasure",
    "enemyVisible",
    "enemyInRange",
    "treasureOnMap"
]

GAMMA = 0.99
MIN_SUPPORT = 3

CSV_PATH = "Logs.csv"
OUTPUT_JSON = "htn_simple.json"

df = pd.read_csv(CSV_PATH, sep=";")

if "reward" in df.columns:
    df["reward"] = df["reward"].astype(str).str.replace(",", ".", regex=False)
    df["reward"] = pd.to_numeric(df["reward"], errors="coerce").fillna(0.0)
else:
    df["reward"] = 0.0

if "action_name" not in df.columns:
    if "action" in df.columns:
        df["action_name"] = df["action"].map(ACTION_MAP)
    else:
        raise ValueError("CSV должен содержать колонку 'action' или 'action_name'")

for c in STATE_FLAGS:
    if c not in df.columns:
        raise ValueError(f"Не найдена колонка состояния '{c}' в CSV")
    df[c] = df[c].astype(int)

compressed = []
for ep, g in df.groupby("episode"):
    g = g.sort_values("step")
    last = None
    for _, row in g.iterrows():
        if row["action_name"] != last:
            compressed.append(row)
            last = row["action_name"]

dfc = pd.DataFrame(compressed).reset_index(drop=True)

def row_to_state_tuple(row):
    return tuple(int(row[c]) for c in STATE_FLAGS)

dfc["state"] = dfc.apply(row_to_state_tuple, axis=1)

df["state"] = df.apply(row_to_state_tuple, axis=1)

transitions = []
for ep, g in dfc.groupby("episode"):
    g = g.sort_values("step").reset_index(drop=True)
    for i in range(len(g)-1):
        transitions.append({
            "episode": ep,
            "index": i,
            "state": g.loc[i, "state"],
            "action": g.loc[i, "action_name"],
            "reward": g.loc[i, "reward"],
            "next_state": g.loc[i+1, "state"]
        })

df_trans = pd.DataFrame(transitions)
print("\n=== Пример сжатых переходов (первые 20) ===\n")
print(tabulate(df_trans.head(20), headers=df_trans.columns, tablefmt="fancy_grid"))

print("\n=== Статистика сжатых переходов ===")
print(f"Всего переходов (сжатые): {len(df_trans)}")
print(f"Эпизоды: {df_trans['episode'].nunique()}")
print(f"Уникальных состояний (сжатые): {df_trans['state'].nunique()}")
print(f"Уникальных (state, action) (сжатые): {df_trans[['state','action']].drop_duplicates().shape[0]}")

transitions_full = []
for ep, g in df.groupby("episode"):
    g = g.sort_values("step").reset_index(drop=True)
    for i in range(len(g)-1):
        transitions_full.append({
            "episode": ep,
            "index": i,
            "state": g.loc[i, "state"],
            "action": g.loc[i, "action_name"],
            "reward": g.loc[i, "reward"],
            "next_state": g.loc[i+1, "state"]
        })

df_trans_full = pd.DataFrame(transitions_full)
print("\n=== Пример полных переходов (первые 20) ===\n")
print(tabulate(df_trans_full.head(20), headers=df_trans_full.columns, tablefmt="fancy_grid"))

print("\n=== Статистика полных переходов ===")
print(f"Всего переходов (полные): {len(df_trans_full)}")
print(f"Эпизоды: {df_trans_full['episode'].nunique()}")
print(f"Уникальных состояний (полные): {df_trans_full['state'].nunique()}")
print(f"Уникальных (state, action) (полные): {df_trans_full[['state','action']].drop_duplicates().shape[0]}")

df_unique_sa = df_trans[["state","action"]].drop_duplicates().reset_index(drop=True)
print("\n=== Уникальные (state, action) (сжатые) ===")
print(tabulate(df_unique_sa, headers=df_unique_sa.columns, tablefmt="fancy_grid"))
print(f"\nВсего уникальных пар (сжатые): {len(df_unique_sa)}\n")

episodes_trans = defaultdict(list)
for tr in transitions:
    episodes_trans[tr["episode"]].append(tr)

for ep, lst in episodes_trans.items():
    lst.sort(key=lambda x: x["index"])
    rewards = [x["reward"] for x in lst]
    G = [0]*len(rewards)
    running = 0.0
    for i in reversed(range(len(rewards))):
        running = rewards[i] + GAMMA * running
        G[i] = running
    for i in range(len(lst)):
        lst[i]["G"] = G[i]

all_trans = []
for ep in episodes_trans:
    all_trans.extend(episodes_trans[ep])

Q_values = defaultdict(list)
next_states = defaultdict(list)

for tr in all_trans:
    key = (tr["state"], tr["action"])
    Q_values[key].append(tr["G"])
    next_states[key].append(tr["next_state"])

Q_mean = {}
Q_count = {}
for key, vals in Q_values.items():
    if len(vals) >= MIN_SUPPORT:
        Q_mean[key] = np.mean(vals)
        Q_count[key] = len(vals)

all_states = sorted(list(set([s for (s, a) in Q_mean.keys()])))
MaxQ = {}
action_prevalence = Counter()

for s in all_states:
    cands = [(a, Q_mean[(s, a)]) for (ss, a) in Q_mean if ss == s]
    if not cands:
        continue
    best_action = max(cands, key=lambda x: x[1])[0]
    MaxQ[s] = best_action
    action_prevalence[best_action] += 1

action_freq = Counter([a for (_, a) in Q_mean.keys()])

action_meanQ = {}
for a in ACTION_MAP.values():
    vals = [Q_mean[(s,x)] for (s,x) in Q_mean if x == a]
    action_meanQ[a] = np.mean(vals) if vals else 0

raw_rows = []
for action in ACTION_MAP.values():
    raw_rows.append([
        action,
        action_prevalence.get(action, 0),
        action_freq.get(action, 0),
        round(action_meanQ.get(action, 0), 3)
    ])

print("\n=== Таблица действий ДО ранжирования ===\n")
print(tabulate(raw_rows, headers=["Action","Prevalence","Frequency","meanQ"], tablefmt="fancy_grid"))

def norm_dict(d):
    if not d:
        return {}
    vals = np.array(list(d.values()), dtype=float)
    mn, mx = vals.min(), vals.max()
    if mx == mn:
        return {k:1.0 for k in d}
    return {k:(v-mn)/(mx-mn) for k,v in d.items()}

np_prev  = norm_dict(action_prevalence)
np_freq  = norm_dict(action_freq)
np_meanQ = norm_dict(action_meanQ)

alpha, beta, gamma = 0.7, 0.1, 0.2
action_score = {}

for a in ACTION_MAP.values():
    action_score[a] = (
        alpha*np_prev.get(a,0) +
        beta*np_freq.get(a,0) +
        gamma*np_meanQ.get(a,0)
    )

ranked_actions = sorted(action_score.items(), key=lambda x: -x[1])

EFFECT_THRESHOLD = 0.5
effects = {}

for action in ACTION_MAP.values():

    ns_list = [tr["next_state"] for tr in transitions if tr["action"] == action]

    if len(ns_list) == 0:
        effects[action] = {
            "support": 0,
            "effect_vector": None,
            "flag_probs": {},
        }
        continue

    N = len(ns_list)

    arr = np.array(ns_list, dtype=int)

    flag_probs = {}
    effect_vector = []

    for idx, flag in enumerate(STATE_FLAGS):
        prob_1 = arr[:, idx].mean()
        flag_probs[flag] = round(prob_1, 3)

        if prob_1 >= EFFECT_THRESHOLD:
            effect_vector.append(1)
        elif prob_1 <= (1 - EFFECT_THRESHOLD):
            effect_vector.append(0)
        else:
            effect_vector.append(None)

    effects[action] = {
        "support": N,
        "effect_vector": tuple(effect_vector),
        "flag_probs": flag_probs
    }

print("\n=== Эффекты (по флагам, с порогом) ===")
for action, eff in effects.items():
    print(f"\n{action}:")
    print(f"  support = {eff['support']}")
    print(f"  effect_vector = {eff['effect_vector']}")
    print(f"  flag_probs = {eff['flag_probs']}")

preconditions = {}

states_by_action_full = defaultdict(list)
for tr in transitions_full:
    states_by_action_full[tr["action"]].append(tr["state"])

for a in ACTION_MAP.values():
    states = states_by_action_full.get(a, [])
    if not states:
        continue
    cond = {}
    N = len(states)
    arr = np.array(states, dtype=int)
    proportions = arr.sum(axis=0) / float(N)

    for i, prop in enumerate(proportions):
        cond[STATE_FLAGS[i]] = int(prop >= 0.5)

    preconditions[a] = {
        "cond_vector": tuple(cond[f] for f in STATE_FLAGS),
        "proportions": {STATE_FLAGS[i]: float(proportions[i]) for i in range(len(STATE_FLAGS))},
        "support": N
    }

print("\n=== Предусловия (по частоте, без сжатия) ===")
for a, p in preconditions.items():
    print(f"{a}: support={p['support']}, proportions={p['proportions']}, cond={p['cond_vector']}")

table_rows = []
for action, score in ranked_actions:
    prev = action_prevalence.get(action, 0)
    freq = action_freq.get(action, 0)
    meanQ = action_meanQ.get(action, 0)

    prec_info = preconditions.get(action, {})
    if prec_info:
        cond_vec = str(prec_info["cond_vector"])
        prec_props = ", ".join(f"{k}={round(v,3)}" for k,v in prec_info["proportions"].items())
        prec_sup = prec_info["support"]
    else:
        cond_vec = "-"
        prec_props = "-"
        prec_sup = 0

    eff_info = effects.get(action, {})
    if eff_info:
        eff_vec = str(eff_info["effect_vector"])
        eff_probs = ", ".join(f"{k}={v}" for k,v in eff_info["flag_probs"].items())
        eff_sup = eff_info["support"]
    else:
        eff_vec = "-"
        eff_probs = "-"
        eff_sup = 0

    table_rows.append([
        action,
        round(score, 3),
        prev,
        freq,
        round(meanQ, 3),
        prec_sup,
        cond_vec,
        prec_props,
        eff_sup,
        eff_vec,
        eff_probs
    ])

print("\n=== Итоговая таблица действий (HTN Analysis) ===\n")
print(tabulate(
    table_rows,
    headers=["Action","Score","Prev","Freq","MeanQ",
             "PrecSup","CondVec","PrecProps",
             "EffSup","EffectVec","EffectProbs"],
    tablefmt="fancy_grid"
))

FINAL_PRECOND = {}
FINAL_EFFECTS = {}

for action in ACTION_MAP.values():

    prec = preconditions.get(action, {})
    eff = effects.get(action, {})

    if not prec or not eff:
        FINAL_PRECOND[action] = {}
        FINAL_EFFECTS[action] = {}
        continue

    prec_props = prec["proportions"]
    eff_probs = eff["flag_probs"]

    important_prec = {}
    important_eff = {}

    for flag, p in prec_props.items():
        if p >= 0.8:
            important_prec[flag] = 1
        elif p <= 0.2:
            important_prec[flag] = 0

    for flag, p in eff_probs.items():

        if p >= 0.63:
            eff_val = 1
        elif p <= 0.2:
            eff_val = 0
        else:
            continue

        if flag in important_prec and important_prec[flag] == eff_val:
            continue

        important_eff[flag] = eff_val

    FINAL_PRECOND[action] = important_prec
    FINAL_EFFECTS[action] = important_eff

final_rows = []
for action, score in ranked_actions:
    final_rows.append([
        action,
        round(score, 3),
        json.dumps(FINAL_PRECOND.get(action, {})),
        json.dumps(FINAL_EFFECTS.get(action, {}))
    ])

print("\n=== ФИНАЛЬНЫЙ HTN (предусловия + эффекты) ===\n")
print(tabulate(
    final_rows,
    headers=["Action", "Score", "FinalPreconditions", "FinalEffects"],
    tablefmt="fancy_grid"
))

output = {
    "ranked_actions": ranked_actions,
    "preconditions_raw": preconditions,
    "effects_raw": effects,
    "final_htn": {
        action: {
            "score": action_score[action],
            "preconditions": FINAL_PRECOND.get(action, {}),
            "effects": FINAL_EFFECTS.get(action, {})
        }
        for action in ACTION_MAP.values()
    }
}

with open(OUTPUT_JSON, "w", encoding="utf-8") as f:
    json.dump(output, f, indent=4, ensure_ascii=False)

print("\n Финальный HTN записан в", OUTPUT_JSON)