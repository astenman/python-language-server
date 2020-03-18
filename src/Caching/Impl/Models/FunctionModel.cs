﻿// Copyright(c) Microsoft Corporation
// All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the License); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED ON AN  *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS
// OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY
// IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABILITY OR NON-INFRINGEMENT.
//
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Python.Analysis.Types;
using Microsoft.Python.Core;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Microsoft.Python.Analysis.Caching.Models {
    [Serializable]
    [DebuggerDisplay("f:{" + nameof(Name) + "}")]
    internal sealed class FunctionModel : CallableModel {
        public OverloadModel[] Overloads { get; set; }
        public FunctionModel() { } // For de-serializer from JSON

        public FunctionModel(IPythonFunctionType func, IServiceContainer services) : base(func, services) {
            Overloads = func.Overloads.Select(s => FromOverload(s, services)).ToArray();
        }

        private static OverloadModel FromOverload(IPythonFunctionOverload o, IServiceContainer services)
            => new OverloadModel {
                Parameters = o.Parameters.Select(p => new ParameterModel {
                    Name = p.Name,
                    Type = p.Type.GetPersistentQualifiedName(services),
                    Kind = p.Kind,
                    DefaultValue = p.DefaultValue.GetPersistentQualifiedName(services),
                }).ToArray(),
                ReturnType = o.StaticReturnValue.GetPersistentQualifiedName(services),
                Documentation = o.Documentation
            };
    }
}
