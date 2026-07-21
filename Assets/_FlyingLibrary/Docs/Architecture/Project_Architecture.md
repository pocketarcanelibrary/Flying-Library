# Flying Library — Project Architecture

Canonical Unity root:

Assets/_FlyingLibrary

## Domains

- Core
- World
- Library
- Workshops
- Discovery
- Creatures
- Interaction
- Audio
- Visual
- UI
- Save
- Tools
- Data
- Docs
- ThirdParty

## Rules

1. Do not create a generic Scripts folder.
2. Organize code by game domain and system.
3. Every file must have a clear responsibility.
4. Third-party assets are never modified directly.
5. Project-owned adapters, wrappers, prefabs, and extensions must isolate external dependencies.
6. Architectural changes require a documented reason.
