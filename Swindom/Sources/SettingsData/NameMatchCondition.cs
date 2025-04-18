﻿namespace Swindom;

/// <summary>
/// 名前の一致条件
/// </summary>
public enum NameMatchCondition : int
{
    /// <summary>
    /// 完全一致
    /// </summary>
    ExactMatch,
    /// <summary>
    /// 部分一致
    /// </summary>
    PartialMatch,
    /// <summary>
    /// 前方一致
    /// </summary>
    ForwardMatch,
    /// <summary>
    /// 後方一致
    /// </summary>
    BackwardMatch,
}
