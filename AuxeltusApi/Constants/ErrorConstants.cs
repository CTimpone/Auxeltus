﻿namespace Auxeltus.Api
{
    public static class ErrorConstants
    {
        //Error Codes
        public const int REQUIRED_CODE = 1;
        public const int MODEL_VALIDATION_CODE = 2;
        public const int UNIQUENESS_CODE = 3;

        //SQL Errors
        public const string ROLE_TITLE_INDEX = "IX_Roles_Title";
    }
}