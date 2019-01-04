using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable IDE1006

namespace FakeUIMS.Models.JSON
{
    public class InputBase
    {
        public string tag { get; set; }
        public string branch { get; set; }
    }

    public class InputObject<TParam> : InputBase
        where TParam : IParams
    {
        public TParam @params { get; set; }
        public int rowLimit { get; set; }
    }

    public interface IParams
    {
    }
}

#pragma warning restore
