using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Duke.Ids4.Models
{
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50)]
        public override string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        public Sex Sex { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50)]
        public string Password { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Sex
    {
        Male,
        Female
    }
}