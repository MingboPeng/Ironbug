using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ladybug_SortByLayers : GH_Component
    {
        public Ladybug_SortByLayers()
          : base("Ladybug_SortByLayers", "SortByLayers",
              "Sort and group Rhino objects by layers.\n\nPlease find the source code from:\nhttps://github.com/MingboPeng/Ironbug",
              "Ladybug", "5 | Extra")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("K", "K", "A list of Rhino objects that associated with sortable layers", GH_ParamAccess.list);
            pManager.AddGeometryParameter("A", "A", "Optional object list to sort synchronously", GH_ParamAccess.list);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("K", "K", "Sorted objects by layers", GH_ParamAccess.tree);
            pManager.AddGeometryParameter("A", "A", "Synchronously sorted objects", GH_ParamAccess.tree);
            pManager.AddTextParameter("n", "n", "Grouped layer names", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //var RefList = Component.Params.Input[0].VolatileData.AllData(true);
            var refList = new List<IGH_GeometricGoo>();
            var secondList = new List<IGH_GeometricGoo>();
            DA.GetDataList(0, refList);
            DA.GetDataList(1, secondList);

            List<string> layerNames = new List<string>();
            var doc = Rhino.RhinoDoc.ActiveDoc;
            var layers = doc.Layers;

            var dic = new Dictionary<string, List<int>>();


            int mark = 0;
            foreach (var item in refList)
            {
                var refID = item.ReferenceID;
                var currentRhinoObj = doc.Objects.Find(refID);
                var atLayerIndex = currentRhinoObj.Attributes.LayerIndex;
                var currentlayerName = layers[atLayerIndex].Name;
                
                //add to layer dictionary
                if (dic.ContainsKey(currentlayerName))
                {
                    dic[currentlayerName].Add(mark);
                }
                else
                {
                    dic[currentlayerName] = new List<int>() { mark };
                }

                mark++;

            }

            var dicKeys = dic.Keys.ToList();
            dicKeys.Sort();

            DataTree<object> treeK = new DataTree<object>();
            DataTree<object> tree_names = new DataTree<object>();
            int i = 0;
            foreach (var layer in dicKeys)
            {
                GH_Path pth = new GH_Path(i);
                foreach (var index in dic[layer])
                {
                    treeK.Add(refList.ElementAt(index), pth);
                }
                tree_names.Add(layer, pth);
                i++;
            }

            DA.SetDataTree(0, treeK);
            DA.SetDataTree(2, tree_names);

            if (secondList.Any())
            {
                DataTree<object> treeA = new DataTree<object>();
                int j = 0;
                foreach (var layer in dicKeys)
                {
                    GH_Path pth = new GH_Path(j);
                    foreach (var index in dic[layer])
                    {
                        treeA.Add(secondList.ElementAt(index), pth);
                    }
                    j++;
                }

                DA.SetDataTree(1,treeA);
                
            }




        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a3b74b0f-2fab-42de-bd88-3326331a93a5"); }
        }
    }
}