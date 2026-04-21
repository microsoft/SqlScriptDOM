---
name: verify-and-test-tsql-syntax
description: Copilot wrapper for the canonical shared skill that verifies exact T-SQL syntax support and adds permanent regression coverage.
argument-hint: "Exact T-SQL script or syntax to verify"
agent: agent
---

Use the [canonical verify-and-test-tsql-syntax skill](../../.agents/skills/verify-and-test-tsql-syntax/SKILL.md) as the source of truth for this workflow.

- Verify the exact T-SQL script first, character for character.
- Use an existing test file for any temporary debug verification.
- If the script already works, add comprehensive permanent tests and baselines.
- If it fails, classify the gap before implementing a fix.
- Treat `TSql180` as the current vNext/latest parser target when the syntax is for vNext or the latest parser.
- Remove temporary debug-only verification code before finishing.

This prompt is a Copilot convenience wrapper around the shared skill. Keep the detailed workflow in the skill, not here.

Input:
${input:tsqlScript:Paste the exact T-SQL script to verify}