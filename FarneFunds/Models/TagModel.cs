using FarneFunds.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarneFunds.Models
{
    public class TagModel
    {
    }

    public class TagList
    {
        public List<TagListItem> ListOfTags { get; private set; }

        public TagList( FarneFundsEntities dal, bool IsActive )
        {
            ListOfTags = new List<TagListItem>();
            ListOfTags.AddRange(dal.Tags.Where(t => t.IsActive == IsActive).OrderBy( o => o.Name ).ToList().Select(s => new TagListItem(s)));
        }
    }

    public class TagListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TagListItem( Tag TagItem )
        {
            Id = TagItem.Id;
            Name = TagItem.Name;
        }
    }

    public class TagDetails
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public TagDetails() { }
        public TagDetails( Tag TagDetail )
        {
            Id = TagDetail.Id;
            Name = TagDetail.Name;
        }
    }
}