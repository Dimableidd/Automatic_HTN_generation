import pandas as pd
import numpy as np
from collections import defaultdict, Counter
import json
from tabulate import tabulate
import os
import re

HTN_OUTPUT_DIR = "htn_generated"
os.makedirs(HTN_OUTPUT_DIR, exist_ok=True)

def extract_htn_actions(rl_agent_path):
    with open(rl_agent_path, "r", encoding="utf-8") as f:
        code = f.read()

    region_match = re.search(
        r"#region\s+HTN_ACTIONS(.*?)#endregion",
        code,
        re.S
    )
    if not region_match:
        raise RuntimeError("Не найден #region HTN_ACTIONS")

    region = region_match.group(1)
    actions = {}

    pos = 0
    while True:
        m = re.search(r"private\s+void\s+(\w+)\s*\(\)", region[pos:])
        if not m:
            break

        name = m.group(1)
        start = pos + m.start()

        brace_start = region.find("{", start)
        brace_count = 0
        i = brace_start

        while i < len(region):
            if region[i] == "{":
                brace_count += 1
            elif region[i] == "}":
                brace_count -= 1
                if brace_count == 0:
                    end = i + 1
                    body = region[brace_start + 1:end - 1].strip()
                    actions[name] = body
                    pos = end
                    break
            i += 1

    return actions

def convert_action_body_to_execute(body):
    lines = body.splitlines()
    out = []
    has_return = False

    for line in lines:
        if re.search(r"\breturn\s*;", line):
            indent = re.match(r"\s*", line).group(0)
            out.append(f"{indent}return TaskResult.FAILURE;")
            has_return = True
        else:
            out.append(line)


    out.append("        return TaskResult.PROCESSING;")

    return out


def extract_htn_sensor_methods(rl_agent_path):
    with open(rl_agent_path, "r", encoding="utf-8") as f:
        code = f.read()

    region_match = re.search(
        r"#region\s+HTN_SENSORS(.*?)#endregion",
        code,
        re.S
    )
    if not region_match:
        raise RuntimeError("Не найден #region HTN_SENSORS")

    region = region_match.group(1)

    methods = []
    pos = 0

    while True:
        m = re.search(r"private\s+bool\s+(\w+)\s*\(\)", region[pos:])
        if not m:
            break

        name = m.group(1)
        start = pos + m.start()

        brace_start = region.find("{", start)
        if brace_start == -1:
            raise RuntimeError(f"Нет тела метода {name}")

        brace_count = 0
        i = brace_start

        while i < len(region):
            if region[i] == "{":
                brace_count += 1
            elif region[i] == "}":
                brace_count -= 1
                if brace_count == 0:
                    end = i + 1
                    method_body = region[start:end].strip()
                    methods.append((name, method_body))
                    pos = end
                    break
            i += 1

    if not methods:
        raise RuntimeError("В HTN_SENSORS нет методов")

    return methods


STATE_FLAGS = {
    "hasTreasure": "HasTreasure",
    "enemyVisible": "IsEnemyVisible",
    "enemyInRange": "IsEnemyInAttackRange",
    "treasureOnMap": "HasTreasureOnMap"
}

def normalize_conditions(cond_dict, allowed_flags):
    fixed = {}

    for k, v in cond_dict.items():
        if k not in allowed_flags:
            raise RuntimeError(
                f"Флаг '{k}' не существует в сенсорах. "
                f"Разрешённые: {sorted(allowed_flags)}"
            )
        fixed[k] = v

    return fixed



def generate_sensors(flags, rl_agent_path):
    methods = extract_htn_sensor_methods(rl_agent_path)

    method_by_name = {name: body for name, body in methods}

    lines = []
    lines.append("using System;")
    lines.append("using UnityEngine;")
    lines.append("")
    lines.append("public class HTNAgentSensors_Auto : HTNAgentSensors")
    lines.append("{")
    lines.append("")
    lines.append("    public override void Initialize(GameObject actor)")
    lines.append("    {")

    for flag in flags:
        lines.append(f'        SetState("{flag}", false, false);')

    lines.append("    }")
    lines.append("")
    lines.append("    private void Update()")
    lines.append("    {")

    for flag, method in flags.items():
        lines.append(f'        SetState("{flag}", {method}(), true);')

    lines.append("    }")
    lines.append("")

    for method_name in flags.values():
        body = method_by_name.get(method_name)
        if body is None:
            raise RuntimeError(f"Метод {method_name} не найден в HTN_SENSORS")

        lines.append("    " + body.replace("\n", "\n    "))
        lines.append("")

    lines.append("}")

    path = os.path.join(HTN_OUTPUT_DIR, "HTNAgentSensors_Auto.cs")
    with open(path, "w", encoding="utf-8") as f:
        f.write("\n".join(lines))


def generate_task(action, prec, eff, action_body_lines):
    cls = f"Task_{action}"

    prec = normalize_conditions(prec, STATE_FLAGS.keys())
    eff  = normalize_conditions(eff,  STATE_FLAGS.keys())

    lines = []
    lines.append("using UnityEngine;")
    lines.append("using System.Collections.Generic;")
    lines.append("")
    lines.append(f'[CreateAssetMenu(fileName = "{cls}", menuName = "HTNTask/{cls}")]')
    lines.append("")
    lines.append(f"public class {cls} : HTNTask")
    lines.append("{")

    lines.append("    public override Dictionary<string, object> PreConditions()")
    lines.append("    {")
    if prec:
        lines.append("        return new Dictionary<string, object>")
        lines.append("        {")
        items = list(prec.items())
        for i, (k, v) in enumerate(items):
            comma = "," if i < len(items) - 1 else ""
            lines.append(f'            {{ "{k}", {"true" if v == 1 else "false"} }}{comma}')
        lines.append("        };")
    else:
        lines.append("        return new Dictionary<string, object>();")
    lines.append("    }")

    lines.append("")
    lines.append("    public override Dictionary<string, object> Effects()")
    lines.append("    {")
    if eff:
        lines.append("        return new Dictionary<string, object>")
        lines.append("        {")
        items = list(eff.items())
        for i, (k, v) in enumerate(items):
            comma = "," if i < len(items) - 1 else ""
            lines.append(f'            {{ "{k}", {"true" if v == 1 else "false"} }}{comma}')
        lines.append("        };")
    else:
        lines.append("        return new Dictionary<string, object>();")
    lines.append("    }")

    lines.append("")
    lines.append("    public override TaskResult Execute(Character character)")
    lines.append("    {")
    for l in action_body_lines:
        lines.append("        " + l)
    lines.append("    }")

    lines.append("}")

    path = os.path.join(HTN_OUTPUT_DIR, f"{cls}.cs")
    with open(path, "w", encoding="utf-8") as f:
        f.write("\n".join(lines))


def generate_domain(ranked_actions, FINAL_PRECOND):
    lines = []

    lines.append("using UnityEngine;")
    lines.append("using System.Collections.Generic;")
    lines.append("")
    lines.append('[CreateAssetMenu(fileName = "HTNDomain_Auto", menuName = "HTNCompoundTask/HTNDomain_Auto")]')
    lines.append("")
    lines.append("public class HTNDomain_Auto : HTNCompoundTask")
    lines.append("{")
    lines.append("    public override List<(Dictionary<string, object> PreConditions, List<HTNTask> Tasks)> GetMethods()")
    lines.append("    {")
    lines.append("        return new List<(Dictionary<string, object>, List<HTNTask>)>")
    lines.append("        {")

    for idx, (action, _) in enumerate(ranked_actions):
        prec = normalize_conditions(
            FINAL_PRECOND.get(action, {}),
            STATE_FLAGS.keys()
        )

        lines.append("            (")

        if prec:
            lines.append("                new Dictionary<string, object>")
            lines.append("                {")
            items = list(prec.items())
            for i, (flag, val) in enumerate(items):
                comma = "," if i < len(items) - 1 else ""
                lines.append(f'                    {{ "{flag}", {"true" if val == 1 else "false"} }}{comma}')
            lines.append("                },")
        else:
            lines.append("                new Dictionary<string, object>(),")

        lines.append("                new List<HTNTask>")
        lines.append("                {")
        lines.append(f"                    CreateInstance<Task_{action}>()")
        lines.append("                }")
        lines.append("            )" + ("," if idx < len(ranked_actions) - 1 else ""))

    lines.append("        };")
    lines.append("    }")
    lines.append("}")

    path = os.path.join(HTN_OUTPUT_DIR, "HTNDomain_Auto.cs")
    with open(path, "w", encoding="utf-8") as f:
        f.write("\n".join(lines))




RL_AGENT_PATH = "RL_Agent.cs"

generate_sensors(STATE_FLAGS, RL_AGENT_PATH)

actions_code = extract_htn_actions("RL_Agent.cs")

for action in ACTION_MAP.values():
    body = actions_code.get(action)
    if not body:
        raise RuntimeError(f"Нет реализации HTN_ACTION {action}")

    exec_lines = convert_action_body_to_execute(body)

    generate_task(
        action,
        FINAL_PRECOND.get(action, {}),
        FINAL_EFFECTS.get(action, {}),
        exec_lines
    )

generate_domain(ranked_actions, FINAL_PRECOND)

print("\nHTN .cs файлы сгенерированы в папке:", HTN_OUTPUT_DIR)