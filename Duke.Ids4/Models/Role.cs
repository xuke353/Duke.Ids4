using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Duke.Ids4.Models
{
    public class Role : IdentityRole<int>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [StringLength(256)]
        [Required]
        public override string Name { get; set; }

        /// <summary>
        /// 标准化后的角色名
        /// </summary>
        [StringLength(256)]
        [Required]
        public override string NormalizedName { get; set; }

        /// <summary>
        /// 一个随机值，每当角色被持久化到数据库时，该值应更改
        /// </summary>
        [StringLength(36)]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 角色描述
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
    }
}