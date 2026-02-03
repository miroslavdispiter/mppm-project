using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

        /// <summary>
        /// Method performs conversion of network elements from CIM based concrete model into DMS model.
        /// </summary>
        private void ConvertModelAndPopulateDelta()
        {
            LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            //// import all concrete model types (DMSType enum)
            //// Redosled je bitan - prvo entiteti bez referenci, pa oni sa referencama
            ImportSeals();
            ImportAssetFunctions();
            ImportManufacturers();
            ImportProductAssetModels();
            ImportAssetOwners();
            ImportAssets();

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
        }

        #region Import

        private void ImportSeals()
        {
            SortedDictionary<string, object> cimSeals = concreteModel.GetAllObjectsOfType("FTN.Seal");

            if (cimSeals == null)
            {
                LogManager.Log("No FTN.Seal objects found.", LogLevel.Info);
                report.Report.AppendLine("No Seal objects found in CIM model.");
                return;
            }

            foreach (KeyValuePair<string, object> cimSealPair in cimSeals)
            {
                FTN.Seal cimSeal = cimSealPair.Value as FTN.Seal;
                if (cimSeal == null)
                {
                    report.Report.AppendLine($"Seal with key={cimSealPair.Key} is null and FAILED to be converted");
                    LogManager.Log($"Null Seal object in dictionary, key={cimSealPair.Key}", LogLevel.Warning);
                    continue;
                }

                ResourceDescription rd = CreateSealResourceDescription(cimSeal);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("Seal ID = ").Append(cimSeal.ID)
                                 .Append(" SUCCESSFULLY converted to GID = ")
                                 .AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("Seal ID = ").Append(cimSeal.ID)
                                 .AppendLine(" FAILED to be converted");
                    LogManager.Log($"Seal with ID {cimSeal.ID} failed to convert (rd null).", LogLevel.Warning);
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateSealResourceDescription(FTN.Seal cimSeal)
        {
            if (cimSeal == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SEAL, importHelper.CheckOutIndexForDMSType(DMSType.SEAL));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cimSeal.ID, gid);

            PowerTransformerConverter.PopulateSealProperties(cimSeal, rd);
            return rd;
        }

        private void ImportAssetFunctions()
        {
            SortedDictionary<string, object> cimAssetFunctions = concreteModel.GetAllObjectsOfType("FTN.AssetFunction");

            if (cimAssetFunctions == null)
            {
                report.Report.AppendLine("No AssetFunction objects found.");
                return;
            }

            foreach (KeyValuePair<string, object> cimAssetFunctionPair in cimAssetFunctions)
            {
                FTN.AssetFunction cimAssetFunction = cimAssetFunctionPair.Value as FTN.AssetFunction;
                if (cimAssetFunction == null)
                {
                    report.Report.AppendLine($"AssetFunction with key={cimAssetFunctionPair.Key} is null and FAILED");
                    LogManager.Log($"Null AssetFunction, key={cimAssetFunctionPair.Key}", LogLevel.Warning);
                    continue;
                }

                ResourceDescription rd = CreateAssetFunctionResourceDescription(cimAssetFunction);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("AssetFunction ID = ").Append(cimAssetFunction.ID)
                                 .Append(" SUCCESSFULLY converted to GID = ")
                                 .AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("AssetFunction ID = ").Append(cimAssetFunction.ID)
                                 .AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateAssetFunctionResourceDescription(FTN.AssetFunction cimAssetFunction)
        {
            if (cimAssetFunction == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ASSETFUNCTION, importHelper.CheckOutIndexForDMSType(DMSType.ASSETFUNCTION));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cimAssetFunction.ID, gid);

            PowerTransformerConverter.PopulateAssetFunctionProperties(cimAssetFunction, rd);
            return rd;
        }

        private void ImportManufacturers()
        {
            SortedDictionary<string, object> cimManufacturers = concreteModel.GetAllObjectsOfType("FTN.Manufacturer");

            if (cimManufacturers == null)
            {
                report.Report.AppendLine("No Manufacturer objects found.");
                return;
            }

            foreach (KeyValuePair<string, object> cimManufacturerPair in cimManufacturers)
            {
                FTN.Manufacturer cimManufacturer = cimManufacturerPair.Value as FTN.Manufacturer;
                if (cimManufacturer == null)
                {
                    report.Report.AppendLine($"Manufacturer with key={cimManufacturerPair.Key} is null");
                    continue;
                }

                ResourceDescription rd = CreateManufacturerResourceDescription(cimManufacturer);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("Manufacturer ID = ").Append(cimManufacturer.ID)
                                 .Append(" SUCCESSFULLY converted GID = ")
                                 .AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("Manufacturer ID = ").Append(cimManufacturer.ID)
                                 .AppendLine(" FAILED");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateManufacturerResourceDescription(FTN.Manufacturer cimManufacturer)
        {
            if (cimManufacturer == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MANUFACTURER, importHelper.CheckOutIndexForDMSType(DMSType.MANUFACTURER));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cimManufacturer.ID, gid);
            PowerTransformerConverter.PopulateManufacturerProperties(cimManufacturer, rd);
            return rd;
        }

        private void ImportProductAssetModels()
        {
            SortedDictionary<string, object> cimProductAssetModels = concreteModel.GetAllObjectsOfType("FTN.ProductAssetModel");

            if (cimProductAssetModels == null)
            {
                report.Report.AppendLine("No ProductAssetModel objects found.");
                return;
            }

            foreach (KeyValuePair<string, object> cimProductAssetModelPair in cimProductAssetModels)
            {
                FTN.ProductAssetModel cimProduct = cimProductAssetModelPair.Value as FTN.ProductAssetModel;
                if (cimProduct == null)
                {
                    report.Report.AppendLine($"ProductAssetModel key={cimProductAssetModelPair.Key} null");
                    continue;
                }

                ResourceDescription rd = CreateProductAssetModelResourceDescription(cimProduct);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("ProductAssetModel ID = ").Append(cimProduct.ID)
                                 .Append(" SUCCESSFULLY converted to GID = ")
                                 .AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("ProductAssetModel ID = ").Append(cimProduct.ID)
                                 .AppendLine(" FAILED");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateProductAssetModelResourceDescription(FTN.ProductAssetModel cimProductAssetModel)
        {
            if (cimProductAssetModel == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PRODUCTASSETMODEL, importHelper.CheckOutIndexForDMSType(DMSType.PRODUCTASSETMODEL));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cimProductAssetModel.ID, gid);

            PowerTransformerConverter.PopulateProductAssetModelProperties(cimProductAssetModel, rd, importHelper, report);
            return rd;
        }

        private void ImportAssetOwners()
        {
            SortedDictionary<string, object> cimAssetOwners = concreteModel.GetAllObjectsOfType("FTN.AssetOwner");

            if (cimAssetOwners == null)
            {
                report.Report.AppendLine("No AssetOwner found.");
                return;
            }

            foreach (KeyValuePair<string, object> cimAssetOwnerPair in cimAssetOwners)
            {
                FTN.AssetOwner cimAssetOwner = cimAssetOwnerPair.Value as FTN.AssetOwner;
                if (cimAssetOwner == null)
                {
                    report.Report.AppendLine($"AssetOwner key={cimAssetOwnerPair.Key} null");
                    continue;
                }

                ResourceDescription rd = CreateAssetOwnerResourceDescription(cimAssetOwner);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("AssetOwner ID = ").Append(cimAssetOwner.ID)
                                 .Append(" SUCCESSFULLY converted to GID = ")
                                 .AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("AssetOwner ID = ").Append(cimAssetOwner.ID)
                                 .AppendLine(" FAILED");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateAssetOwnerResourceDescription(FTN.AssetOwner cimAssetOwner)
        {
            if (cimAssetOwner == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ASSETOWNER, importHelper.CheckOutIndexForDMSType(DMSType.ASSETOWNER));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cimAssetOwner.ID, gid);

            PowerTransformerConverter.PopulateAssetOwnerProperties(cimAssetOwner, rd);
            return rd;
        }

        private void ImportAssets()
        {
            SortedDictionary<string, object> cimAssets = concreteModel.GetAllObjectsOfType("FTN.Asset");

            if (cimAssets == null)
            {
                report.Report.AppendLine("No Asset objects found.");
                return;
            }

            foreach (KeyValuePair<string, object> assetPair in cimAssets)
            {
                FTN.Asset cimAsset = assetPair.Value as FTN.Asset;
                if (cimAsset == null)
                {
                    report.Report.AppendLine($"Asset key={assetPair.Key} null");
                    continue;
                }

                ResourceDescription rd = CreateAssetResourceDescription(cimAsset);
                if (rd != null)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                    report.Report.Append("Asset ID = ").Append(cimAsset.ID)
                                 .Append(" SUCCESSFULLY converted to GID = ")
                                 .AppendLine(rd.Id.ToString());
                }
                else
                {
                    report.Report.Append("Asset ID = ").Append(cimAsset.ID)
                                 .AppendLine(" FAILED to be converted");
                }
            }
            report.Report.AppendLine();
        }

        private ResourceDescription CreateAssetResourceDescription(FTN.Asset cimAsset)
        {
            if (cimAsset == null) return null;

            long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ASSET, importHelper.CheckOutIndexForDMSType(DMSType.ASSET));
            ResourceDescription rd = new ResourceDescription(gid);
            importHelper.DefineIDMapping(cimAsset.ID, gid);

            PowerTransformerConverter.PopulateAssetProperties(cimAsset, rd, importHelper, report);
            return rd;
        }
        #endregion Import
    }
}

