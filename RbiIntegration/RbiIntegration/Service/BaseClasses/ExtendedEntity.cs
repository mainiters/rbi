using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Расширение для коробочной сущности
    /// </summary>
    public class ExtendedEntity
    {
        public bool IsJustCreated { get; set; }

        public Entity Entity { get; set; }

        public ExtendedEntity(Entity source, bool isJustCreated = false)
        {
            this.Entity = source;
            this.IsJustCreated = isJustCreated;
        }
    }
}
