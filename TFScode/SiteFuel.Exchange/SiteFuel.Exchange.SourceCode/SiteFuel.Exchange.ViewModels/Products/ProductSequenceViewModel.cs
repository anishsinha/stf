using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public static class ProductSequenceModelMapper
    {
        public static ProductSequenceModel ToClone(this ProductSequenceModel instance)
        {
            return new ProductSequenceModel()
            {
                ProductTypeId = instance.ProductTypeId,
                DisplayName = instance.DisplayName,
                OrderId = instance.OrderId,
                JobId = instance.JobId,
                Sequence = instance.Sequence
            };
        }
    }
    public class ProductSequenceViewModel
    {
        public List<ProductSequenceModel> ProductSequence { get; set; } = new List<ProductSequenceModel>();
        public ProductSequenceType SequenceType { get; set; } = ProductSequenceType.Product;
        public List<int> ProductIds { get; set; } = new List<int>();
        public ProductSequencingCreationMethod SequenceMethod { get; set; } = ProductSequencingCreationMethod.Account;
        public int JobId { get; set; }
    }

    public class ProductSequenceModel
    {
        public int? ProductTypeId { get; set; }
        public string DisplayName { get; set; }
        public int Sequence { get; set; }
        public int? OrderId { get; set; }
        public int? JobId { get; set; }
    }

    public class DisplaySequenceModel
    {
        public List<DropdownDisplayExtendedId> DisplayListSeq { get; set; } = new List<DropdownDisplayExtendedId>();
        public List<DropdownDisplayExtendedId> SelectedSeq { get; set; } = new List<DropdownDisplayExtendedId>();
    }
}
