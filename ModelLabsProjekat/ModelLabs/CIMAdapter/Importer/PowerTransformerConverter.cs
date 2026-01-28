namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

        #region Populate ResourceDescription
        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.AliasNameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
                }
            }
        }

        public static void PopulateSealProperties(FTN.Seal cimSeal, ResourceDescription rd)
        {
            if ((cimSeal != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimSeal, rd);

                if (cimSeal.ConditionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEAL_CONDITION, (short)GetDMSSealConditionKind(cimSeal.Condition)));
                }
                if (cimSeal.KindHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEAL_KIND, (short)GetDMSSealKind(cimSeal.Kind)));
                }
                if (cimSeal.SealNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEAL_SEALNUMBER, cimSeal.SealNumber));
                }
            }
        }

        public static void PopulateAssetFunctionProperties(FTN.AssetFunction cimAssetFunction, ResourceDescription rd)
        {
            if ((cimAssetFunction != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimAssetFunction, rd);

                if (cimAssetFunction.ConfigIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSETFUNCTION_CONFIGID, cimAssetFunction.ConfigID));
                }
                if (cimAssetFunction.FirmwareIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSETFUNCTION_FIRMWAREID, cimAssetFunction.FirmwareID));
                }
                if (cimAssetFunction.HardwareIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSETFUNCTION_HARDWAREID, cimAssetFunction.HardwareID));
                }
                if (cimAssetFunction.PasswordHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSETFUNCTION_PASSWORD, cimAssetFunction.Password));
                }
                if (cimAssetFunction.ProgramIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSETFUNCTION_PROGRAMID, cimAssetFunction.ProgramID));
                }
            }
        }

        public static void PopulateOrganisationRoleProperties(FTN.OrganisationRole cimOrganisationRole, ResourceDescription rd)
        {
            if ((cimOrganisationRole != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimOrganisationRole, rd);
            }
        }

        public static void PopulateManufacturerProperties(FTN.Manufacturer cimManufacturer, ResourceDescription rd)
        {
            if ((cimManufacturer != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateOrganisationRoleProperties(cimManufacturer, rd);
            }
        }

        public static void PopulateAssetOrganisationRoleProperties(FTN.AssetOrganisationRole cimAssetOrganisationRole, ResourceDescription rd)
        {
            if ((cimAssetOrganisationRole != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateOrganisationRoleProperties(cimAssetOrganisationRole, rd);
            }
        }

        public static void PopulateAssetOwnerProperties(FTN.AssetOwner cimAssetOwner, ResourceDescription rd)
        {
            if ((cimAssetOwner != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateAssetOrganisationRoleProperties(cimAssetOwner, rd);
            }
        }

        public static void PopulateAssetModelProperties(FTN.AssetModel cimAssetModel, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimAssetModel != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimAssetModel, rd);
            }
        }

        public static void PopulateProductAssetModelProperties(FTN.ProductAssetModel cimProductAssetModel, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimProductAssetModel != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateAssetModelProperties(cimProductAssetModel, rd, importHelper, report);

                if (cimProductAssetModel.CorporateStandardKindHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PRODUCTASSETMODEL_CORPORATESTANDARDKIND, (short)GetDMSCorporateStandardKind(cimProductAssetModel.CorporateStandardKind)));
                }
                if (cimProductAssetModel.ModelNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PRODUCTASSETMODEL_MODELNUMBER, cimProductAssetModel.ModelNumber));
                }
                if (cimProductAssetModel.ModelVersionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PRODUCTASSETMODEL_MODELVERSION, cimProductAssetModel.ModelVersion));
                }
                if (cimProductAssetModel.UsageKindHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PRODUCTASSETMODEL_USAGEKIND, (short)GetDMSAssetModelUsageKind(cimProductAssetModel.UsageKind)));
                }
                if (cimProductAssetModel.ManufacturerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimProductAssetModel.Manufacturer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimProductAssetModel.GetType().ToString()).Append(" rdfID = \"").Append(cimProductAssetModel.ID);
                        report.Report.Append("\" - Failed to set reference to Manufacturer: rdfID \"").Append(cimProductAssetModel.Manufacturer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.PRODUCTASSETMODEL_MANUFACTURER, gid));
                }
            }
        }

        public static void PopulateAssetProperties(FTN.Asset cimAsset, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimAsset != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimAsset, rd);

                if (cimAsset.CriticalHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSET_CRITICAL, cimAsset.Critical));
                }
                if (cimAsset.InitialConditionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSET_INITIALCONDITION, cimAsset.InitialCondition));
                }
                if (cimAsset.LotNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSET_LOTNUMBER, cimAsset.LotNumber));
                }
                if (cimAsset.SerialNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSET_SERIALNUMBER, cimAsset.SerialNumber));
                }
                if (cimAsset.TypeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSET_TYPE, cimAsset.Type));
                }
                if (cimAsset.UtcNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASSET_UTCNUMBER, cimAsset.UtcNumber));
                }
            }
        }
        #endregion Populate ResourceDescription

        #region Enums convert
        public static SealConditionKind GetDMSSealConditionKind(FTN.SealConditionKind sealConditionKind)
        {
            switch (sealConditionKind)
            {
                case FTN.SealConditionKind.broken:
                    return SealConditionKind.broken;
                case FTN.SealConditionKind.locked:
                    return SealConditionKind.locked;
                case FTN.SealConditionKind.missing:
                    return SealConditionKind.missing;
                case FTN.SealConditionKind.open:
                    return SealConditionKind.open;
                case FTN.SealConditionKind.other:
                    return SealConditionKind.other;
                default:
                    return SealConditionKind.other;
            }
        }

        public static SealKind GetDMSSealKind(FTN.SealKind sealKind)
        {
            switch (sealKind)
            {
                case FTN.SealKind.lead:
                    return SealKind.lead;
                case FTN.SealKind.@lock:
                    return SealKind.@lock;
                case FTN.SealKind.steel:
                    return SealKind.steel;
                case FTN.SealKind.other:
                    return SealKind.other;
                default:
                    return SealKind.other;
            }
        }

        public static CorporateStandardKind GetDMSCorporateStandardKind(FTN.CorporateStandardKind corporateStandardKind)
        {
            switch (corporateStandardKind)
            {
                case FTN.CorporateStandardKind.experimental:
                    return CorporateStandardKind.experimental;
                case FTN.CorporateStandardKind.other:
                    return CorporateStandardKind.other;
                case FTN.CorporateStandardKind.standard:
                    return CorporateStandardKind.standard;
                case FTN.CorporateStandardKind.underEvaluation:
                    return CorporateStandardKind.underEvaluation;
                default:
                    return CorporateStandardKind.other;
            }
        }

        public static AssetModelUsageKind GetDMSAssetModelUsageKind(FTN.AssetModelUsageKind assetModelUsageKind)
        {
            switch (assetModelUsageKind)
            {
                case FTN.AssetModelUsageKind.customerSubstation:
                    return AssetModelUsageKind.customerSubstation;
                case FTN.AssetModelUsageKind.distributionOverhead:
                    return AssetModelUsageKind.distributionOverhead;
                case FTN.AssetModelUsageKind.distributionUnderground:
                    return AssetModelUsageKind.distributionUnderground;
                case FTN.AssetModelUsageKind.other:
                    return AssetModelUsageKind.other;
                case FTN.AssetModelUsageKind.streetlight:
                    return AssetModelUsageKind.streetlight;
                case FTN.AssetModelUsageKind.substation:
                    return AssetModelUsageKind.substation;
                case FTN.AssetModelUsageKind.transmission:
                    return AssetModelUsageKind.transmission;
                case FTN.AssetModelUsageKind.unknown:
                    return AssetModelUsageKind.unknown;
                default:
                    return AssetModelUsageKind.unknown;
            }
        }
		#endregion Enums convert
	}
}
