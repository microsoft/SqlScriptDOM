---
name: new-feature-implementation
description: Copilot wrapper for the canonical add-feature skill used to implement new SqlScriptDOM T-SQL features.
argument-hint: "Feature description or exact T-SQL syntax"
agent: agent
---

Use the [canonical add-feature skill](../../.agents/skills/add-feature/SKILL.md) as the source of truth for this workflow.

- Implement the requested SQL Server feature end to end.
- Start by asking the discovery questions defined in the shared skill.
- Classify the request into the correct feature type before editing code.
- Follow the matching file in `.github/instructions/` for implementation, tests, and validation.
- Treat `TSql180` as the current vNext/latest parser target.
- Use the exact T-SQL syntax supplied by the user when creating tests.

This prompt is a Copilot convenience wrapper around the shared skill. Keep the detailed workflow in the skill, not here.

Input:
${input:featureDescription:Describe the feature or paste the exact T-SQL syntax}