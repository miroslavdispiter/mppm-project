using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{
    public enum DMSType : short
    {
        MASK_TYPE = unchecked((short)0xFFFF),

        SEAL = 0x0001,
        ASSETFUNCTION = 0x0002,
        ASSET = 0x0003,
        MANUFACTURER = 0x0004,
        ASSETOWNER = 0x0005,
        PRODUCTASSETMODEL = 0x0006,
    }

    [Flags]
    public enum ModelCode : long
    {
        // ===================================================================
        // IDENTIFIEDOBJECT (apstraktna bazna klasa)
        // ===================================================================
        IDOBJ = 0x1000000000000000,
        IDOBJ_GID = 0x1000000000000104,
        IDOBJ_MRID = 0x1000000000000207,
        IDOBJ_NAME = 0x1000000000000307,
        IDOBJ_ALIASNAME = 0x1000000000000407,

        // ===================================================================
        // SEAL (konkretna, nasledjuje IDOBJ)
        // ===================================================================
        SEAL = 0x1100000000010000,
        SEAL_SEALNUMBER = 0x1100000000010107,
        SEAL_CONDITION = 0x110000000001020A,
        SEAL_KIND = 0x110000000001030A,

        // ===================================================================
        // ASSETFUNCTION (konkretna, nasledjuje IDOBJ)
        // ===================================================================
        ASSETFUNCTION = 0x1200000000020000,
        ASSETFUNCTION_CONFIGID = 0x1200000000020107,
        ASSETFUNCTION_FIRMWAREID = 0x1200000000020207,
        ASSETFUNCTION_HARDWAREID = 0x1200000000020307,
        ASSETFUNCTION_PASSWORD = 0x1200000000020407,
        ASSETFUNCTION_PROGRAMID = 0x1200000000020507,

        // ===================================================================
        // ASSET (konkretna, nasledjuje IDOBJ)
        // ===================================================================
        ASSET = 0x1300000000030000,
        ASSET_CRITICAL = 0x1300000000030101,
        ASSET_INITIALCONDITION = 0x1300000000030207,
        ASSET_LOTNUMBER = 0x1300000000030307,
        ASSET_SERIALNUMBER = 0x1300000000030407,
        ASSET_TYPE = 0x1300000000030507,
        ASSET_UTCNUMBER = 0x1300000000030607,
        ASSET_ASSETINFO = 0x1300000000030709,           // referenca na AssetInfo
        ASSET_ORGANISATIONROLES = 0x1300000000030819,   // lista AssetOrganisationRole

        // ===================================================================
        // ASSETINFO (byreference klasa, nasledjuje IDOBJ)
        // ===================================================================
        ASSETINFO = 0x1600000000000000,
        ASSETINFO_ASSETMODEL = 0x1600000000000109,      // referenca na AssetModel

        // ===================================================================
        // ORGANISATIONROLE (apstraktna, nasledjuje IDOBJ)
        // ===================================================================
        ORGANISATIONROLE = 0x1400000000000000,

        // ===================================================================
        // MANUFACTURER (konkretna, nasledjuje ORGANISATIONROLE)
        // ===================================================================
        MANUFACTURER = 0x1410000000040000,
        MANUFACTURER_PRODUCTASSETMODEL = 0x1410000000040109,

        // ===================================================================
        // ASSETORGANISATIONROLE (apstraktna, nasledjuje ORGANISATIONROLE)
        // ===================================================================
        ASSETORGANISATIONROLE = 0x1420000000000000,

        // ===================================================================
        // ASSETOWNER (konkretna, nasledjuje ASSETORGANISATIONROLE)
        // ===================================================================
        ASSETOWNER = 0x1430000000050000,

        // ===================================================================
        // ASSETMODEL (apstraktna, nasledjuje IDOBJ)
        // ===================================================================
        ASSETMODEL = 0x1500000000000000,

        // ===================================================================
        // PRODUCTASSETMODEL (konkretna, nasledjuje ASSETMODEL)
        // ===================================================================
        PRODUCTASSETMODEL = 0x1510000000060000,
        PRODUCTASSETMODEL_MODELNUMBER = 0x1510000000060107,
        PRODUCTASSETMODEL_MODELVERSION = 0x1510000000060207,
        PRODUCTASSETMODEL_CORPORATESTANDARDKIND = 0x151000000006030A,
        PRODUCTASSETMODEL_USAGEKIND = 0x151000000006040A,
        PRODUCTASSETMODEL_MANUFACTURER = 0x1510000000060509,
    }

    [Flags]
    public enum ModelCodeMask : long
    {
        MASK_TYPE = 0x00000000ffff0000,
        MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
        MASK_ATTRIBUTE_TYPE = 0x00000000000000ff,

        MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
        MASK_FIRSTNBL = unchecked((long)0xf000000000000000),
        MASK_DELFROMNBL8 = unchecked((long)0xfffffff000000000),
    }
}