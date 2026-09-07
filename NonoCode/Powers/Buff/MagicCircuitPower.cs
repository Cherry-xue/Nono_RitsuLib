using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Nono.NonoCode.SecondaryResources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Nono.NonoCode.Powers;

// 魔力回路-每打出3张魔法牌,获得1魔力
public sealed class MagicCircuitPower : NonoPower
{
    private class Data
    {
        public int magiccardPlayed;
        public int triggerCount;
    }
    //定义内部数据类，包含已打出的魔法牌数量和触发次数
    private const int _magiccardIncrement = 3;
    //定义魔法牌触发增量为3
    public override PowerType Type => PowerType.Buff;
    //定义能力类型：增益
    public override PowerStackType StackType => PowerStackType.Counter;
    //定义叠加类型：计数器
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    //定义实例类型：单例
    public override int DisplayAmount => 3 - GetInternalData<Data>().magiccardPlayed % 3;
    //定义显示数值：3减去已打出的魔法牌数量对3取余的结果
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromKeyword(NonoKeywords.MagicCard),
    ];
    //定义额外的悬停提示：显示魔法牌关键字的悬停提示
    protected override object InitInternalData()
    {
        return new Data();
    }
    //初始化内部数据：返回一个新的Data对象
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || !cardPlay.Card.Keywords.Contains(NonoKeywords.MagicCard))
        {
            return;
        }
        Data data = GetInternalData<Data>();
        data.magiccardPlayed++;
        int triggers = data.magiccardPlayed / 3 - data.triggerCount;
        if (triggers > 0)
        {
            Flash();
            await SecondaryResourceCmd.Gain(Owner.Player, ModResources.ManaId, Amount * triggers);
            data.triggerCount += triggers;
        }
        InvokeDisplayAmountChanged();
    }
    //定义在卡牌打出后触发的效果：如果打出的卡牌是魔法牌，则增加已打出的魔法牌数量，并计算触发次数，如果触发次数大于0，则闪烁能力图标并获得魔力，最后更新显示数值
}
