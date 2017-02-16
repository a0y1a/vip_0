using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace KeKeSoftPlatform.Common
{
    public class DropDownListAttribute:ListAttribute
    {
        public DropDownListAttribute(string listName)
            : base(listName)
        {

        }

        public override void OnMetadataCreated(ModelMetadata metadata)
        {
            base.OnMetadataCreated(metadata);
            metadata.TemplateHint = "DropDownList";
        }
    }
}
