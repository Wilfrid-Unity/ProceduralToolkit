using System.Collections.Generic;
using ProceduralToolkit.Examples.UI;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Configurator for BookshelfGenerator with UI and editor controls
    /// </summary>
    public class BookshelfGeneratorConfigurator : ConfiguratorBase
    {
        public MeshFilter platformMeshFilter;
        public MeshFilter bookshelfMeshFilter;
        public MeshRenderer bookshelfMeshRenderer;
        public Material bookshelfCaseMaterial;
        public Material bookMaterial;

        public RectTransform leftPanel;
        public bool constantSeed = false;
        public BookshelfGenerator.Config config = new BookshelfGenerator.Config();

        private const float minInternalHeight = 0.03f;
        private const float maxInternalHeight = 5.20f;

        private const float minInternalWidth =  0.03f;
        private const float maxInternalWidth =  5.20f;

        private const float minInternalDepth = 0.03f;
        private const float maxInternalDepth = 2.00f;

        private const float minPlanksWidth = 0.03f;
        private const float maxPlanksWidth = 0.15f;

        private const int minShelvesCount = 0;
        private const int maxShelvesCount = 12;

        private const float platformHeight = 0.05f;
        private const float platformRadiusOffset = 0.20f;

        private Mesh bookshelfMesh;
        private Mesh platformMesh;

        private void Awake()
        {
            Generate();
            SetupSkyboxAndPalette();

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Height", minInternalHeight, maxInternalHeight, config.internalHeight, value =>
                {
                    config.internalHeight = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Width", minInternalWidth, maxInternalWidth, config.internalWidth, value =>
                {
                    config.internalWidth = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Depth", minInternalDepth, maxInternalDepth, config.internalDepth, value =>
                {
                    config.internalDepth = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Planks Width", minPlanksWidth, maxPlanksWidth, config.planksWidth, value =>
                {
                    config.planksWidth = value;
                    Generate();
                });
            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Number of Shelves", minShelvesCount, maxShelvesCount, config.shelvesCount, value =>
                {
                    config.shelvesCount = value;
                    Generate();
                });
#if false
            InstantiateControl<ToggleControl>(leftPanel).Initialize("Has stretchers", config.hasStretchers, value =>
            {
                config.hasStretchers = value;
                Generate();
            });
#endif
            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", () => Generate(true));
        }

        private void Update()
        {
            UpdateSkybox();
        }

        public void Generate(bool randomizeConfig = false)
        {
            if (constantSeed)
            {
                Random.InitState(0);
            }

            if (randomizeConfig)
            {
                GeneratePalette();

                config.color = GetMainColorHSV().ToColor();

                config.internalHeight = Random.Range(minInternalHeight, maxInternalHeight);
                config.internalWidth = Random.Range(minInternalWidth, maxInternalWidth);
                config.internalDepth = Random.Range(minInternalDepth, maxInternalDepth);
                config.planksWidth = Random.Range(minPlanksWidth, maxPlanksWidth);
                config.shelvesCount = Random.Range(minShelvesCount, maxShelvesCount);
            }

            float platformRadius = Geometry.GetCircumradius(config.internalWidth, config.internalDepth) + platformRadiusOffset;
            var platformDraft = Platform(platformRadius, platformHeight);
            AssignDraftToMeshFilter(platformDraft, platformMeshFilter, ref platformMesh);

            var bookshelfDraft = BookshelfGenerator.Bookshelf(config);
            AssignDraftToMeshFilter(bookshelfDraft, bookshelfMeshFilter, ref bookshelfMesh);

            //bookshelfMeshFilter.sharedMesh = bookshelfDraft.ToMeshWithSubMeshes();

            var materials = new List<Material>();
            foreach (var draft in bookshelfDraft)
            {
                if (draft.name == "Bookshelf case")
                {
                    materials.Add(bookshelfCaseMaterial);
                }
                else if (draft.name == "Book")
                {
                    materials.Add(bookMaterial);
                }
            }
            bookshelfMeshRenderer.sharedMaterials = materials.ToArray();
        }
    }
}
