# AGENTS.md

This repository keeps its canonical Copilot instruction files in the `.github` folder and its reusable agent skills in `.agents/skills`.

Before starting work, read `.github/copilot-instructions.md` for repository-wide guidance.

Then use the relevant topic-specific instruction files in `.github/instructions/`:
- `adding_new_parser.guidelines.instructions.md`
- `bug_fixing.guidelines.instructions.md`
- `database_option.guidelines.instructions.md`
- `debugging_workflow.guidelines.instructions.md`
- `function.guidelines.instructions.md`
- `grammar_validation.guidelines.instructions.md`
- `grammar.guidelines.instructions.md`
- `new_data_types.guidelines.instructions.md`
- `new_index_types.guidelines.instructions.md`
- `parser.guidelines.instructions.md`
- `testing.guidelines.instructions.md`

When a reusable skill applies, load it from `.agents/skills/`:
- `add-feature/SKILL.md`
- `verify-and-test-tsql-syntax/SKILL.md`

Version note:
- Treat `TSql180` as the current vNext/latest parser target in agent workflows unless a more specific repo instruction overrides it.

Selection rule:
- Use only the instruction file or files that match the task.
- If multiple files apply, follow all of them.
- If there is any conflict, treat `.github/copilot-instructions.md` as the repository-wide baseline and then apply the more specific file from `.github/instructions/` for the current task.

Do not duplicate, restate, or replace the maintained guidance in this file. This file is only a redirect to the instruction and skill sources above.
