﻿using System;

namespace TrueFill.SCIM2.Model
{
    public class SchemasHelper
    {
        public enum Schema { ListResponse, User }

        public static string[] Get(Schema schema)
        {
            switch (schema)
            {
                case Schema.ListResponse:
                    return new string[1] { "urn:ietf:params:scim:api:messages:2.0:ListResponse" };
                case Schema.User:
                    return new string[1] { "urn:ietf:params:scim:schemas:core:2.0:User" };
                default:
                    throw new Exception("unknown schema");
            }
        }
    }
}
