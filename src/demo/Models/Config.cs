﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models;
public class Config
{
    public string DeploymentName { get; set; }
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }
    public string ModelId { get; set; }
    public string ServiceId { get; set; }
}
