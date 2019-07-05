using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A fully procedural bookshelf generator, creates entire mesh from scratch and paints it's vertices
    /// </summary>
    public static class BookshelfGenerator
    {
        [Serializable]
        public class Config
        {
            public float internalHeight = 1.5f;
            public float internalWidth  = 0.7f;
            public float internalDepth  = 0.2f;
            public float planksWidth = 0.05f;
            public Color color = Color.white;
        }

        public static MeshDraft Bookshelf(Config config)
        {
            var bookshelf = new MeshDraft {name = "Bookshelf"};

            {
                var bottomPlank = MeshDraft.Hexahedron(
                    config.internalWidth + 2 * config.planksWidth,
                    config.internalDepth + config.planksWidth,
                    config.planksWidth, false);
                bottomPlank.Move(Vector3.up * (config.planksWidth) / 2);
                bookshelf.Add(bottomPlank);
            }

            {
                var backPlank = MeshDraft.Hexahedron(
                    config.internalWidth + 2 * config.planksWidth,
                    config.planksWidth,
                    config.internalHeight, false);
                backPlank.Move(Vector3.up * (config.internalHeight/2 + config.planksWidth));
                backPlank.Move(Vector3.forward * config.internalDepth/2);
                bookshelf.Add(backPlank);
            }

            {
                var leftPlank = MeshDraft.Hexahedron(config.planksWidth, config.internalDepth, config.internalHeight, false);
                leftPlank.Move(Vector3.up * (config.internalHeight/2  + config.planksWidth));
                leftPlank.Move(Vector3.left * (config.internalWidth/2 + config.planksWidth/ 2));
                leftPlank.Move(Vector3.back * config.planksWidth/2);
                bookshelf.Add(leftPlank);
            }

            {
                var rightPlank = MeshDraft.Hexahedron(config.planksWidth, config.internalDepth, config.internalHeight, false);
                rightPlank.Move(Vector3.up * (config.internalHeight/2 + config.planksWidth));
                rightPlank.Move(Vector3.right * (config.internalWidth/2 + config.planksWidth/ 2));
                rightPlank.Move(Vector3.back * config.planksWidth/2);
                bookshelf.Add(rightPlank);
            }

            {
                var topPlank = MeshDraft.Hexahedron(
                    config.internalWidth + 2 * config.planksWidth,
                    config.internalDepth + config.planksWidth,
                    config.planksWidth, false);
                topPlank.Move(Vector3.up * (config.internalHeight + 3 * config.planksWidth / 2));
                bookshelf.Add(topPlank);
            }

            bookshelf.Paint(config.color);

            return bookshelf;
        }
    }
}
