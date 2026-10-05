# eQuantic.Core

## Specs (OpenSpec)

This repository plans in the central store,
[eQuantic/equantic-specs](https://github.com/eQuantic/equantic-specs), as
[the `core` workstream](https://github.com/eQuantic/equantic-specs/blob/main/workstreams/core.md):
its specs live under `openspec/specs/core/`, and its changes under `openspec/changes/`, each named
`core-<what-it-delivers>`.
`openspec/config.yaml` here only points there, so `/opsx:propose`, `/opsx:apply` and the `openspec`
CLI run here act on the store. Pull it before starting (`git -C ../equantic-specs pull --rebase`),
and follow its `CLAUDE.md` for how a change flows. Never create `openspec/specs` or
`openspec/changes` here: a local planning root would quietly take this repository off the store.

Apply a change by its name (`/opsx:apply <change>`): the store holds every product's changes, and an
apply with no name can pick another workstream's.

On a machine without the store, set it up once beside this repository, with OpenSpec 1.14.0 or later
(`npm install -g @fission-ai/openspec@1.14.0`): `git clone git@github.com:eQuantic/equantic-specs.git
../equantic-specs`, then `openspec store register ../equantic-specs`. `openspec doctor` checks the
whole chain and prints the fix for whatever is missing.
