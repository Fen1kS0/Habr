﻿namespace Habr.Common.Exceptions;

public class BusinessException : BaseException
{
    public BusinessException(string message) : base(message, 400)
    {
    }
}