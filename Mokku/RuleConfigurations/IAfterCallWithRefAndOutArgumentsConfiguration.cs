﻿namespace Mokku.RuleConfigurations;

public interface IAfterCallWithRefAndOutArgumentsConfiguration<out IReturnType> : IAfterCallConfiguration<IReturnType>, IRefAndOutArgumentsConfiguration<IReturnType>;