using System;

namespace Mohmd.AspNetCore.Proxify.Exmaple.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}