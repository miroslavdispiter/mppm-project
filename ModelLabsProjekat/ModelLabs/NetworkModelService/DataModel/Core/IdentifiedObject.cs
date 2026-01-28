using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public enum TypeOfReference : short
    {
        Reference = 1,  // Referenca koju ovaj objekat ima na druge
        Target = 2,     // Referenca koju drugi objekti imaju na ovaj
        Both = 3        // Oba tipa referenci
    }

    public class IdentifiedObject
    {
        private long globalId;
        private string mRID = string.Empty;
        private string name = string.Empty;
        private string aliasName = string.Empty;

        public IdentifiedObject(long globalId)
        {
            this.globalId = globalId;
        }

        public long GlobalId => globalId;
        public string MRID { get => mRID; set => mRID = value; }
        public string Name { get => name; set => name = value; }
        public string AliasName { get => aliasName; set => aliasName = value; }

        public override bool Equals(object obj)
        {
            if (obj is IdentifiedObject x)
            {
                return globalId == x.globalId &&
                       mRID == x.mRID &&
                       name == x.name &&
                       aliasName == x.aliasName;
            }
            return false;
        }

        public override int GetHashCode() => globalId.GetHashCode();

        #region IAccess implementation

        public virtual bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.IDOBJ_GID:
                case ModelCode.IDOBJ_MRID:
                case ModelCode.IDOBJ_NAME:
                case ModelCode.IDOBJ_ALIASNAME:
                    return true;
                default:
                    return false;
            }
        }

        public virtual void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.IDOBJ_GID: prop.SetValue(globalId); break;
                case ModelCode.IDOBJ_MRID: prop.SetValue(mRID); break;
                case ModelCode.IDOBJ_NAME: prop.SetValue(name); break;
                case ModelCode.IDOBJ_ALIASNAME: prop.SetValue(aliasName); break;
            }
        }

        public virtual void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.IDOBJ_MRID: mRID = property.AsString(); break;
                case ModelCode.IDOBJ_NAME: name = property.AsString(); break;
                case ModelCode.IDOBJ_ALIASNAME: aliasName = property.AsString(); break;
            }
        }

        public virtual Property GetProperty(ModelCode modelCode)
        {
            Property p = new Property(modelCode);
            GetProperty(p);
            return p;
        }

        public virtual bool IsReferenced => false;
        public virtual void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType) { }
        public virtual void AddReference(ModelCode referenceId, long globalId) { }
        public virtual void RemoveReference(ModelCode referenceId, long globalId) { }

        #endregion
    }
}
