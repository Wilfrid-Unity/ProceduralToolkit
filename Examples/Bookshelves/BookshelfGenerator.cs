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
            public int shelvesCount = 3;

            public float booksDensity = 0.5f;
            public float booksThickness = 0.03f;
            public float booksHeight = 0.30f;
            public float booksWidth = 0.20f;

            public Color color = Color.white;
        }

        struct Book
        {
            public float thickness;
            public float linearPosition;
            public Color color;
        };

        private static float HeightPerShelf(Config config)
        {
            return config.internalHeight / (config.shelvesCount + 1);
        }

        private static float ShelfVerticalPosition(Config config, int i /*shelfIndex*/)
        {
            float heightPerShelf = HeightPerShelf(config);
            return (config.planksWidth / 2 + (i+1) * ( heightPerShelf + config.planksWidth) );
        }

        private static MeshDraft BookshelfCase(Config config)
        {
            var bookshelfCase = new MeshDraft {name = "Bookshelf case"};

            {
                var bottomPlank = MeshDraft.Hexahedron(
                    config.internalWidth + 2 * config.planksWidth,
                    config.internalDepth + config.planksWidth,
                    config.planksWidth, false);
                bottomPlank.Move(Vector3.up * (config.planksWidth) / 2);
                bookshelfCase.Add(bottomPlank);
            }

            float internalHeightWithShelves = config.internalHeight + config.shelvesCount * config.planksWidth;

            {
                var backPlank = MeshDraft.Hexahedron(
                    config.internalWidth + 2 * config.planksWidth,
                    config.planksWidth,
                    internalHeightWithShelves, false);
                backPlank.Move(Vector3.up * (internalHeightWithShelves/2 + config.planksWidth));
                backPlank.Move(Vector3.forward * config.internalDepth/2);
                bookshelfCase.Add(backPlank);
            }

            {
                var leftPlank = MeshDraft.Hexahedron(config.planksWidth, config.internalDepth, internalHeightWithShelves, false);
                leftPlank.Move(Vector3.up * (internalHeightWithShelves/2  + config.planksWidth));
                leftPlank.Move(Vector3.left * (config.internalWidth/2 + config.planksWidth/ 2));
                leftPlank.Move(Vector3.back * config.planksWidth/2);
                bookshelfCase.Add(leftPlank);
            }

            {
                var rightPlank = MeshDraft.Hexahedron(config.planksWidth, config.internalDepth, internalHeightWithShelves, false);
                rightPlank.Move(Vector3.up * (internalHeightWithShelves/2 + config.planksWidth));
                rightPlank.Move(Vector3.right * (config.internalWidth/2 + config.planksWidth/ 2));
                rightPlank.Move(Vector3.back * config.planksWidth/2);
                bookshelfCase.Add(rightPlank);
            }

            {
                var topPlank = MeshDraft.Hexahedron(
                    config.internalWidth + 2 * config.planksWidth,
                    config.internalDepth + config.planksWidth,
                    config.planksWidth, false);
                topPlank.Move(Vector3.up * (config.planksWidth / 2 + internalHeightWithShelves + config.planksWidth));
                bookshelfCase.Add(topPlank);
            }

            
            for (int i = 0; i < config.shelvesCount; ++i)
            {
                var shelf = MeshDraft.Hexahedron(config.internalWidth, config.internalDepth, config.planksWidth, false);
                shelf.Move(Vector3.up * ShelfVerticalPosition(config, i) );
                shelf.Move(Vector3.back * config.planksWidth/2);
                bookshelfCase.Add(shelf);
            }

            bookshelfCase.Paint(config.color);

            return bookshelfCase;
        }

        public static CompoundMeshDraft Bookshelf(Config config)
        {
            var bookshelf = new CompoundMeshDraft{name = "Bookshelf"};

            // case
            {
                var bookshelfCase = BookshelfCase(config);
#if true // comment-out here to debug more easily
                bookshelf.Add(bookshelfCase);
#endif
            }

            // books
            float availableBookshelfWidth = config.internalWidth * config.shelvesCount * config.booksDensity;
            int booksCount = (int)(availableBookshelfWidth / config.booksThickness);
            if (booksCount > 0)
            {
                Book[] books = new Book[booksCount];
                float currentBookLinearPosition = 0f;
                for (int i = 0; i < booksCount; ++i)
                {
                    books[i] = new Book
                    {
                        thickness = config.booksThickness,
                        linearPosition = currentBookLinearPosition + UnityEngine.Random.Range(0f, config.booksThickness * (1f - config.booksDensity)),
                        color = UnityEngine.Random.ColorHSV(),
                    };
                    currentBookLinearPosition = books[i].linearPosition + books[i].thickness;
                }

                for (int i = 0; i < /*2*/ booksCount; ++i)
                {
                    Book book = books[i];
                    int shelfIndex = (int)(book.linearPosition % config.internalWidth);
                    float positionOnShelf = book.linearPosition - shelfIndex * config.internalWidth;

                    var bookMesh = MeshDraft.Hexahedron(
                        config.booksThickness,
                        config.booksWidth,
                        config.booksHeight,
                        false);
                    bookMesh.name = "Book";
                    bookMesh.Move(Vector3.up * (config.booksHeight) / 2);
                    bookMesh.Move(Vector3.up * ShelfVerticalPosition(config, i) );
                    bookMesh.Move(Vector3.left * (config.internalWidth) / 2);
                    bookMesh.Move(Vector3.right * positionOnShelf);

                    bookMesh.Paint(book.color);
                    bookshelf.Add(bookMesh);
                }
            }

            return bookshelf;
        }
    }
}
