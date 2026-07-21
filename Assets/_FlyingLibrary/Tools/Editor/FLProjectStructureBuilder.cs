#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace FlyingLibrary.Tools.Editor
{
    /// <summary>
    /// Создаёт и проверяет каноническую структуру проекта Flying Library.
    /// Инструмент остаётся в проекте и может запускаться повторно без риска.
    /// </summary>
    public static class FLProjectStructureBuilder
    {
        private const string ProjectRoot = "Assets/_FlyingLibrary";

        private static readonly string[] CanonicalFolders =
        {
            "Core",
            "World",
            "Library",
            "Workshops",
            "Discovery",
            "Creatures",
            "Interaction",
            "Audio",
            "Visual",
            "UI",
            "Save",
            "Tools",
            "Tools/Editor",
            "Data",
            "Docs",
            "Docs/Constitution",
            "Docs/Architecture",
            "Docs/Decisions",
            "Docs/Sprints",
            "ThirdParty"
        };

        [MenuItem("Flying Library/Project/Create Canonical Structure")]
        public static void CreateCanonicalStructure()
        {
            EnsureFolderExists(ProjectRoot);

            foreach (string relativeFolder in CanonicalFolders)
            {
                string fullPath = $"{ProjectRoot}/{relativeFolder}";
                EnsureFolderExists(fullPath);
            }

            CreateInitialDocumentation();

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            Debug.Log(
                "Flying Library: canonical project structure created successfully."
            );

            EditorUtility.DisplayDialog(
                "Flying Library",
                "Canonical project structure created successfully.",
                "OK"
            );
        }

        [MenuItem("Flying Library/Project/Validate Canonical Structure")]
        public static void ValidateCanonicalStructure()
        {
            int missingCount = 0;

            foreach (string relativeFolder in CanonicalFolders)
            {
                string fullPath = $"{ProjectRoot}/{relativeFolder}";

                if (!AssetDatabase.IsValidFolder(fullPath))
                {
                    Debug.LogWarning(
                        $"Flying Library: missing folder: {fullPath}"
                    );

                    missingCount++;
                }
            }

            if (missingCount == 0)
            {
                EditorUtility.DisplayDialog(
                    "Flying Library",
                    "The canonical structure is complete.",
                    "OK"
                );

                Debug.Log(
                    "Flying Library: canonical structure validation passed."
                );
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Flying Library",
                    $"Missing folders found: {missingCount}. Check the Console.",
                    "OK"
                );
            }
        }

        private static void EnsureFolderExists(string fullPath)
        {
            fullPath = fullPath.Replace("\\", "/");

            if (AssetDatabase.IsValidFolder(fullPath))
            {
                return;
            }

            string parentPath = Path.GetDirectoryName(fullPath)
                ?.Replace("\\", "/");

            string folderName = Path.GetFileName(fullPath);

            if (string.IsNullOrWhiteSpace(parentPath))
            {
                Debug.LogError(
                    $"Flying Library: invalid folder path: {fullPath}"
                );

                return;
            }

            if (!AssetDatabase.IsValidFolder(parentPath))
            {
                EnsureFolderExists(parentPath);
            }

            AssetDatabase.CreateFolder(parentPath, folderName);
        }

        private static void CreateInitialDocumentation()
        {
            CreateMarkdownFileIfMissing(
                $"{ProjectRoot}/Docs/Constitution/01_Vision.md",
                GetVisionDocument()
            );

            CreateMarkdownFileIfMissing(
                $"{ProjectRoot}/Docs/Constitution/02_Core_Principles.md",
                GetCorePrinciplesDocument()
            );

            CreateMarkdownFileIfMissing(
                $"{ProjectRoot}/Docs/Architecture/Project_Architecture.md",
                GetArchitectureDocument()
            );

            CreateMarkdownFileIfMissing(
                $"{ProjectRoot}/Docs/Decisions/ADR-0001-Discovery-First.md",
                GetDiscoveryFirstDecision()
            );

            CreateMarkdownFileIfMissing(
                $"{ProjectRoot}/Docs/Sprints/Sprint-001-Room-001.md",
                GetSprint001Document()
            );
        }

        private static void CreateMarkdownFileIfMissing(
            string assetPath,
            string content
        )
        {
            string projectDirectory = Directory.GetParent(
                Application.dataPath
            )!.FullName;

            string absolutePath = Path.Combine(
                projectDirectory,
                assetPath
            );

            absolutePath = absolutePath.Replace("/", Path.DirectorySeparatorChar.ToString());

            if (File.Exists(absolutePath))
            {
                return;
            }

            string? directory = Path.GetDirectoryName(absolutePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(absolutePath, content);
        }

        private static string GetVisionDocument()
        {
            return
@"# Flying Library — Vision

Flying Library creates conditions in which players make discoveries for themselves.

The project does not begin with facts, laws, tests, or explanations. It begins with curiosity, interaction, experimentation, surprise, and personal experience.

The player does not merely consume the Library. The player helps it grow.

## Core promise

The player should feel:

- I noticed something.
- I tried something.
- Something unexpected happened.
- I understood a new relationship.
- I want to experiment again.

## Development direction

The project grows one polished room, one workshop, and one discovery at a time.

Technology, graphics, animation, lighting, sound, and tools may evolve.

The soul of the project does not.
";
        }

        private static string GetCorePrinciplesDocument()
        {
            return
@"# Flying Library — Core Principles

## 1. Discovery before explanation

Create conditions for the player to discover relationships independently.

## 2. Experience before fact

The player first interacts, experiments, observes, and understands. Names and facts come later, only when useful.

## 3. Show instead of tell

Use objects, materials, movement, sound, light, reaction, and environment instead of unnecessary text.

## 4. Minimum interface

Whenever possible, buttons become physical objects, menus become books or mechanisms, and progression becomes a visible change in the world.

## 5. Historical causality

Nothing appears before its prerequisites exist.

## 6. Local room evolution

Only the room or workshop connected to newly discovered knowledge changes.

## 7. Personal library history

Objects made by the player remain in the world and create a unique biography of that player's Library.

## 8. Natural materials and tactility

Objects should feel touchable through visible texture, believable weight, sound, and movement.

## 9. Stable core, evolving technology

Rendering, graphics, video, AI tools, shaders, animation, and performance may improve without changing the game itself.

## 10. Protection from feature creep

New ideas must strengthen player-driven discovery or be postponed.
";
        }

        private static string GetArchitectureDocument()
        {
            return
@"# Flying Library — Project Architecture

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
";
        }

        private static string GetDiscoveryFirstDecision()
        {
            return
@"# ADR-0001 — Discovery First

## Status

Accepted.

## Decision

Flying Library is designed around player-driven discovery rather than direct instruction.

The canonical sequence is:

Curiosity → Experiment → Discovery → Understanding

Facts and terminology may appear after experience, but they must not replace experience.

## Consequences

- Mechanics are evaluated by what the player can discover.
- Long explanatory tutorials are avoided.
- Environmental communication is preferred.
- Failure and accidents may reveal new possibilities.
- Workshops reproduce simplified paths through which knowledge and crafts developed.
";
        }

        private static string GetSprint001Document()
        {
            return
@"# Sprint 001 — Room 001: Law and Order

## Goal

Create the first stable, playable room of Flying Library.

## Initial scope

- Octagonal room
- Stone walls
- Wooden floor
- High segmented ceiling
- Massive old wooden beams
- Antique windows
- Fireplace position
- Rotating bookshelf system foundation
- Atmospheric lighting foundation
- Room structure suitable for future books, Curie, magical dust, birds, and sound

## Excluded from the first architectural milestone

- Additional rooms
- Workshops
- Full book sorting
- Curie final character model
- Progression systems
- Complex UI
- Dialogue
- Economy
- Achievements

## Completion rule

Room 001 must exist as a stable foundation that later systems can extend without rebuilding its architecture.
";
        }
    }
}

#endif