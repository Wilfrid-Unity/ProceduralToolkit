using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [CustomEditor(typeof(BookshelfGeneratorConfigurator))]
    public class BookshelfGeneratorConfiguratorEditor : UnityEditor.Editor
    {
        private BookshelfGeneratorConfigurator generator;

        private void OnEnable()
        {
            generator = (BookshelfGeneratorConfigurator) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.bookshelfMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate bookshelf");
                generator.Generate(randomizeConfig: false);
            }
            if (GUILayout.Button("Randomize config and generate mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.bookshelfMeshFilter,
                    generator.platformMeshFilter,
                }, "Generate bookshelf");
                generator.Generate(randomizeConfig: true);
            }
        }
    }
}
