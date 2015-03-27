using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ToolbAR.Vuforia
{
    public static class ARUtilities
    {
        public const string QCAR_PATH_RELATIVE = "/StreamingAssets/QCAR";
        static public List<string> DataSetResources = new List<string>();
        static public List<string> getDataSetResources()
        {
            DataSetResources = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/StreamingAssets/QCAR");
            foreach (FileInfo fi in info.GetFiles("*.dat"))
            {
                string setname = Path.GetFileNameWithoutExtension(fi.Name);
                if (DataSet.Exists(setname))
                {
                    DataSetResources.Add(setname);
                }
            }
            return DataSetResources;
        }

        static public List<string> getStoredDataSets()
        {
            List<string> results = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/StreamingAssets/QCAR");
            foreach (FileInfo fi in info.GetFiles("*.dat"))
            {
                string setname = Path.GetFileNameWithoutExtension(fi.Name);
                results.Add(setname);
            }
            return results;
        }

        static public DataSetInformation getStoredDataSetInformation(string name)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.dataPath + "/StreamingAssets/QCAR/" + name + ".xml");
            return DataSetInformation.fromXML(doc, name);
        }
        //Returns a list of updated or created gameobjects
        static public List<GameObject> updateTrackablesFor(ARDataSetBehaviour dataset)
        {
            List<GameObject> list = new List<GameObject>();
            if (dataset.Name == null)
                return list;

            DataSetInformation dsInfo = getStoredDataSetInformation(dataset.Name);
            foreach (ImageTargetInformation itInfo in dsInfo.ImageTargets)
            {
                ImageTargetBehaviour it = null;
                foreach (var existing in dataset.transform.GetComponentsInChildren<ImageTargetBehaviour>())
                {
                    if (existing.TrackableName == itInfo.Name)
                    {
                        it = existing;
                        break;
                    }
                }
                if (it == null)
                {
                    //No such target found, so create a new one
                    GameObject go = new GameObject("ARImageTarget<" + itInfo.Name + ">");
                    go.transform.parent = dataset.transform;
                    go.transform.position = Vector3.zero;
                    go.transform.rotation = Quaternion.identity;
                    go.AddComponent<ARTrackableBehaviour>();
                    it = go.AddComponent<ImageTargetBehaviour>();
                }
                updateImageTarget(it, dsInfo, itInfo);
                list.Add(it.gameObject);
            }

            return list;
        }

        static public List<DataSet> getActiveDatasets()
        {
            List<DataSet> datasets = new List<DataSet>();
            foreach (DataSet dataset in ARScene.Instance.QCARImageTracker.GetActiveDataSets())
            {
                datasets.Add(dataset);
            }
            return datasets;
        }

        static void updateImageTarget(ImageTargetBehaviour it, DataSetInformation dsInfo, ImageTargetInformation itInfo)
        {
            IEditorImageTargetBehaviour itEditor = it as IEditorImageTargetBehaviour;
            itEditor.SetInitializedInEditor(true);
            itEditor.SetImageTargetType(ImageTargetType.PREDEFINED);
            itEditor.SetDataSetPath("QCAR/" + dsInfo.Name + ".xml");
            itEditor.SetNameForTrackable(itInfo.Name);
            itEditor.SetHeight(itInfo.Height);
            itEditor.SetWidth(itInfo.Width);
        }

        //Displays information of a DataSet stored in the XMLs
        public class DataSetInformation
        {
            private DataSetInformation() { }

            public List<ImageTargetInformation> ImageTargets;

            public string Name;

            static public DataSetInformation fromXML(XmlDocument doc, string name)
            {
                DataSetInformation info = new DataSetInformation();
                info.Name = name;
                XmlNode nTracking = doc.DocumentElement.SelectSingleNode("/QCARConfig/Tracking");
                if (nTracking != null)
                {
                    info.ImageTargets = new List<ImageTargetInformation>();
                    foreach (XmlNode node in nTracking.ChildNodes)
                    {
                        switch (node.LocalName)
                        {
                            case "ImageTarget":
                                info.ImageTargets.Add(ImageTargetInformation.fromXMLNode(node));
                                break;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Could not get /QCARConfig/Tracking in " + name + ".xml");
                }

                return info;
            }
        }

        public class ImageTargetInformation
        {
            private ImageTargetInformation() { }

            public string Name;
            public float Width;
            public float Height;

            static public ImageTargetInformation fromXMLNode(XmlNode node)
            {
                ImageTargetInformation info = new ImageTargetInformation();

                info.Name = node.Attributes["name"].InnerText;
                string[] size = node.Attributes["size"].InnerText.Split(' ');
                info.Width = float.Parse(size[0]);
                info.Height = float.Parse(size[1]);

                return info;
            }
        }

        public class DataSetManager
        {
            public class StoredInfo
            {
                DataSet mDataSet = null;
                string mName = "";

                public bool Active = false;
                public DataSet DataSet
                {
                    get
                    {
                        return mDataSet;
                    }
                }
                public string Name
                {
                    get
                    {
                        return mName;
                    }
                }

                public StoredInfo(string name, DataSet dataset)
                {
                    mDataSet = dataset;
                    mName = name;
                }
            }

            //Keeps track of all loaded datasets, to avoid duplicates
            private static Dictionary<string, StoredInfo> LoadedDataSets = new Dictionary<string, StoredInfo>();

            //Returns true if dataset is active, false if it is unloaded or inactive
            public static bool isActive(string name)
            {
                StoredInfo info = null;
                if (LoadedDataSets.TryGetValue(name, out info))
                {
                    return info.Active;
                }
                else return false;
            }

            //Activates a dataset if possbile
            public static bool activate(string name)
            {
                StoredInfo info = null;
                if (LoadedDataSets.TryGetValue(name, out info))
                {
                    if (info.Active)
                        return false;

                    if (ARScene.Instance.QCARImageTracker.ActivateDataSet(info.DataSet))
                    {
                        info.Active = true;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            public static bool deactivate(string name)
            {
                StoredInfo info = null;
                if (LoadedDataSets.TryGetValue(name, out info))
                {
                    if (!info.Active)
                        return false;

                    if (ARScene.Instance.QCARImageTracker.DeactivateDataSet(info.DataSet))
                    {
                        info.Active = false;
                        return true;
                    }
                    else return false;

                }
                else return false;
            }

            public static StoredInfo loadOrGetDataSet(string name, ImageTracker QCARImageTracker)
            {
                StoredInfo info = null;
                if (LoadedDataSets.TryGetValue(name, out info))
                {
                    return info;
                }
                else
                {
                    DataSet dataSet = QCARImageTracker.CreateDataSet();
                    if (!DataSet.Exists(name) || !dataSet.Load(name))
                        return null;
                    else
                    {
                        info = new StoredInfo(name, dataSet);
                        LoadedDataSets.Add(name, info);
                        return info;
                    }
                }
            }

            public static bool unloadDataSet(string name, ImageTracker QCARImageTracker)
            {
                StoredInfo info = null;
                if (LoadedDataSets.TryGetValue(name, out info))
                {
                    if (info.Active)
                        deactivate(name);

                    if (QCARImageTracker.DestroyDataSet(info.DataSet, false))
                    {
                        LoadedDataSets.Remove(name);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else return false;
            }
        }

    }
}