using UnityEngine;

namespace ChemistryLab.Desktop
{
    public enum LabLanguage
    {
        Vietnamese = 0,
        English = 1,
        ChineseSimplified = 2
    }

    public static class LabLocalization
    {
        private const string LanguagePreferenceKey = "chemistryLab.desktop.language";
        private const string MobileChineseMigrationKey = "chemistryLab.mobile.chineseMigration.v3";

        public static LabLanguage Current
        {
            get
            {
                if (Application.isMobilePlatform
                    && PlayerPrefs.GetInt(MobileChineseMigrationKey, 0) == 0)
                {
                    PlayerPrefs.SetInt(LanguagePreferenceKey, (int)LabLanguage.ChineseSimplified);
                    PlayerPrefs.SetInt(MobileChineseMigrationKey, 1);
                    PlayerPrefs.Save();
                }

                var defaultLanguage = Application.isMobilePlatform
                    ? LabLanguage.ChineseSimplified
                    : LabLanguage.Vietnamese;
                var raw = PlayerPrefs.GetInt(LanguagePreferenceKey, (int)defaultLanguage);
                if (raw == (int)LabLanguage.ChineseSimplified) return LabLanguage.ChineseSimplified;
                if (raw == (int)LabLanguage.English) return LabLanguage.English;
                return LabLanguage.Vietnamese;
            }
            set
            {
                PlayerPrefs.SetInt(LanguagePreferenceKey, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static bool IsEnglish { get { return Current == LabLanguage.English; } }
        public static bool IsChinese { get { return Current == LabLanguage.ChineseSimplified; } }
        public static bool UsesEnglishPresentation { get { return IsEnglish || IsChinese; } }

        public static string Text(string vietnamese, string english)
        {
            if (IsChinese) return TranslateEnglish(english);
            return IsEnglish ? english : vietnamese;
        }

        public static string Text(string vietnamese, string english, string chinese)
        {
            if (IsChinese) return chinese;
            return IsEnglish ? english : vietnamese;
        }

        public static void Toggle()
        {
            switch (Current)
            {
                case LabLanguage.ChineseSimplified:
                    Current = LabLanguage.English;
                    break;
                case LabLanguage.English:
                    Current = LabLanguage.Vietnamese;
                    break;
                default:
                    Current = LabLanguage.ChineseSimplified;
                    break;
            }
        }

        public static string ChemicalName(string id, string fallback)
        {
            if (!IsChinese || string.IsNullOrWhiteSpace(id)) return fallback;
            switch (id)
            {
                case "water": return "蒸馏水";
                case "sodium-chloride": return "氯化钠";
                case "hydrochloric-acid": return "盐酸";
                case "sodium-hydroxide": return "氢氧化钠";
                case "copper-sulfate": return "五水硫酸铜";
                case "sulfuric-acid": return "硫酸";
                case "potassium-permanganate": return "高锰酸钾";
                case "barium-chloride": return "二水氯化钡";
                case "silver-nitrate": return "硝酸银";
                case "potassium-iodide": return "碘化钾";
                case "lead-nitrate": return "硝酸铅";
                case "iron-chloride": return "六水氯化铁";
                case "ammonia": return "氨水";
                case "hydrogen-peroxide": return "过氧化氢";
                case "acetic-acid": return "乙酸";
                case "calcium-carbonate": return "碳酸钙";
                case "zinc": return "锌";
                case "copper": return "铜";
                case "magnesium": return "镁";
                case "manganese-dioxide": return "二氧化锰";
                case "nitric-acid": return "硝酸";
                case "phosphoric-acid": return "磷酸";
                case "potassium-hydroxide": return "氢氧化钾";
                case "calcium-hydroxide": return "氢氧化钙";
                case "barium-hydroxide": return "氢氧化钡";
                case "sodium-carbonate": return "碳酸钠";
                case "sodium-bicarbonate": return "碳酸氢钠";
                case "calcium-chloride": return "氯化钙";
                case "copper-chloride": return "二水氯化铜";
                case "iron-sulfate": return "七水硫酸亚铁";
                case "aluminium-chloride": return "六水氯化铝";
                case "ammonium-chloride": return "氯化铵";
                case "sodium-sulfate": return "硫酸钠";
                case "potassium-nitrate": return "硝酸钾";
                case "sodium-sulfide": return "硫化钠";
                case "aluminium": return "铝";
                case "iron": return "铁";
                case "sodium-phosphate": return "磷酸钠";
                case "calcium-oxide": return "氧化钙";
                case "potassium-bromide": return "溴化钾";
                default: return fallback;
            }
        }

        public static string PhaseLabel(ChemicalPhase phase, string fallback)
        {
            if (!IsChinese) return fallback;
            switch (phase)
            {
                case ChemicalPhase.Liquid: return "液体";
                case ChemicalPhase.Aqueous: return "水溶液";
                case ChemicalPhase.Gas: return "气体";
                default: return "固体";
            }
        }

        public static string FamilyLabel(ChemicalDefinition chemical, string fallback)
        {
            if (!IsChinese || chemical == null) return fallback;
            var id = chemical.Id ?? string.Empty;
            if (id == "water") return "溶剂";
            if (id.EndsWith("-acid")) return "酸";
            if (id.EndsWith("-hydroxide") || id == "ammonia") return "碱";
            if (chemical.ModelKind == ChemicalModelKind.Metal) return "金属";
            if (id.EndsWith("-oxide") || id == "hydrogen-peroxide") return "氧化物 / 过氧化物";
            return "盐";
        }

        public static string ChemicalData(string id, string field, string fallback)
        {
            if (!IsChinese) return fallback;

            // Starter chemicals get fully readable data because they are the first mobile mission.
            if (id == "sodium-hydroxide")
            {
                switch (field)
                {
                    case "density": return "2.13 g/cm³";
                    case "melting": return "318 °C";
                    case "boiling": return "1388 °C";
                    case "appearance": return "白色固体，吸湿性很强";
                    case "solubility": return "易溶于水并明显放热";
                    case "hazards": return "强腐蚀性；可造成皮肤和眼睛灼伤";
                    case "handling": return "密封保存；缓慢加入水中，切勿把水直接倒在大量固体上";
                    case "use": return "酸碱反应与氢氧化物沉淀实验";
                    case "reactivity": return "强碱；可中和酸，并与多种金属盐形成难溶氢氧化物。";
                }
            }

            if (id == "copper-sulfate")
            {
                switch (field)
                {
                    case "density": return "2.28 g/cm³";
                    case "melting": return "约 110 °C 起失去结晶水";
                    case "boiling": return "沸腾前分解";
                    case "appearance": return "蓝色晶体";
                    case "solubility": return "约 32 g/100 mL H₂O（20 °C）";
                    case "hazards": return "吞咽有害；对水生环境有危害";
                    case "handling": return "佩戴手套；含铜废物应单独收集";
                    case "use": return "制备蓝色 Cu(OH)₂ 沉淀";
                    case "reactivity": return "铜(II)盐；与强碱反应可生成蓝色氢氧化铜沉淀。";
                }
            }

            if (id == "water")
            {
                switch (field)
                {
                    case "appearance": return "无色透明液体";
                    case "solubility": return "可与多种极性物质混合";
                    case "hazards": return "常规实验条件下风险较低";
                    case "handling": return "保持器具清洁，避免离子污染";
                    case "use": return "溶剂与对照样品";
                    case "reactivity": return "极性溶剂与常用反应介质。";
                }
            }

            if (id == "hydrochloric-acid")
            {
                switch (field)
                {
                    case "appearance": return "无色透明溶液，浓溶液可发烟";
                    case "solubility": return "与水完全混溶";
                    case "hazards": return "腐蚀性；蒸气会刺激呼吸道";
                    case "handling": return "佩戴护目镜和手套；浓溶液在通风橱内操作";
                    case "use": return "酸碱反应及制备氯化物";
                    case "reactivity": return "强酸；中和碱，与碳酸盐放出 CO₂，并可与活泼金属反应。";
                }
            }

            if (id == "sulfuric-acid")
            {
                switch (field)
                {
                    case "appearance": return "无色黏稠液体";
                    case "solubility": return "与水混合并强烈放热";
                    case "hazards": return "强腐蚀性；与水和有机物接触可能剧烈放热";
                    case "handling": return "始终将酸缓慢加入水中，并做好防护";
                    case "use": return "酸性反应、硫酸盐反应与催化";
                    case "reactivity": return "强酸；稀释时大量放热，可参与中和和沉淀反应。";
                }
            }

            return TranslateVietnameseRuntime(fallback);
        }

        public static string TranslateVietnameseRuntime(string value)
        {
            if (!IsChinese || string.IsNullOrWhiteSpace(value)) return value;
            return value
                .Replace("Không đủ tín dụng để mua mặt nạ lọc độc.", "积分不足，无法购买防毒面罩。")
                .Replace("Đã mua và đeo mặt nạ lọc độc. Bộ lọc không thay thế tủ hút.", "已购买并佩戴防毒面罩。过滤器不能替代通风橱。")
                .Replace("Đã đeo mặt nạ lọc độc.", "已佩戴防毒面罩。")
                .Replace("Đã tháo mặt nạ lọc độc.", "已摘下防毒面罩。")
                .Replace("Đã nối bình cách ly khí vào cốc trong tủ hút.", "已连接通风橱气体捕集装置。")
                .Replace("Đã tháo bình cách ly khí.", "已断开气体捕集装置。")
                .Replace("Ca trực an toàn", "安全值班")
                .Replace("Chưa ghi nhận phơi nhiễm.", "尚未记录暴露事件。")
                .Replace("Không phát tán độc chất", "未释放有毒物质")
                .Replace("Phản ứng không tạo nguy cơ khí đáng kể trong mô hình hiện tại.", "当前模型中该反应未产生显著气体风险。")
                .Replace("Khí đã được kiểm soát", "气体风险已受控")
                .Replace("Bất tỉnh · sơ tán khẩn cấp", "失去意识 · 紧急撤离")
                .Replace("Phơi nhiễm ", "暴露于 ")
                .Replace("Phản ứng diễn ra ngoài tủ hút.", "反应在通风橱外进行。")
                .Replace("Tủ hút đã giảm phát tán nhưng cấu hình bảo vệ chưa đủ.", "通风橱降低了扩散，但当前防护仍不足。")
                .Replace("Quạt tủ hút đang tắt nên không có khả năng hút khí.", "通风橱风机已关闭，无法有效排出气体。")
                .Replace("sức khỏe", "生命值")
                .Replace("tín dụng", "积分")
                .Replace("Phân hủy", "分解")
                .Replace("Khoảng ", "约 ")
                .Replace("Tinh thể trắng", "白色晶体")
                .Replace("Bột trắng", "白色粉末")
                .Replace("Chất lỏng không màu", "无色液体")
                .Replace("Tan tốt trong nước", "易溶于水")
                .Replace("Tan nhiều trong nước", "易溶于水")
                .Replace("Không tan trong nước", "不溶于水")
                .Replace("Ăn mòn", "腐蚀性")
                .Replace("Độc khi nuốt", "吞咽有毒")
                .Replace("Nguy cơ thấp", "风险较低")
                .Replace("Đeo găng", "佩戴手套")
                .Replace("Tránh tạo bụi", "避免扬尘");
        }

        public static string TranslateEnglish(string value)
        {
            if (!IsChinese || string.IsNullOrWhiteSpace(value)) return value;

            switch (value)
            {
                case "SAFE": return "安全";
                case "LOCKED": return "已锁定";
                case "WARNING": return "警告";
                case "INTERACT": return "交互";
                case "INSPECT": return "分析";
                case "PUT AWAY": return "收起样品";
                case "PAUSE": return "暂停";
                case "MISSION": return "任务";
                case "DILUTE": return "稀释";
                case "COLLECT": return "收集";
                case "INVENTORY": return "库存";
                case "HEAT +": return "升温 +";
                case "COOL -": return "降温 -";
                case "GUIDE": return "指南";
                case "SKIP": return "跳过";
                case "SETTINGS": return "设置";
                case "BACK": return "返回";
                case "START / CONTINUE": return "开始 / 继续";
                case "START / RESUME": return "开始 / 继续";
                case "EXIT APP": return "退出应用";
                case "MAIN MENU": return "主菜单";
                case "ANALYSIS": return "分析";
                case "CLOSE": return "关闭";
                case "BATCH": return "批次";
                case "ON": return "开";
                case "OFF": return "关";
                case "BUY · ": return "购买 · ";
                case "REMOVE": return "摘下";
                case "WEAR": return "佩戴";
                case "DISCONNECT": return "断开";
                case "CONNECT": return "连接";
                case "Fume hood": return "通风橱";
                case "Wash station": return "清洗区";
                case "Chemical storage": return "化学品储存区";
                case "Analysis bench": return "分析台";
                case "Reaction bench": return "反应台";
                case "Create blue Cu(OH)₂ precipitate": return "制备蓝色 Cu(OH)₂ 沉淀";
                case "Equation not identified": return "尚未识别反应方程式";
                case "No sample in hand": return "手中没有样品";
                case "No catalyst required": return "无需催化剂";
                case "The vessel is ready.": return "反应容器已就绪。";
                case "Waiting for another reagent or a required condition.": return "等待另一种试剂或必要反应条件。";
                case "No supported reaction is predicted for the current mixture.": return "当前混合物未预测到受支持的反应。";
                case "Reaction blocked. Adjust the required conditions or safety controls.": return "反应被阻止。请调整反应条件或安全措施。";
                case "A solid precipitate forms. Observe the product colour and settling.": return "生成固体沉淀。请观察颜色和沉降现象。";
                case "Gas is released. Keep the vessel in the fume hood and use the gas trap.": return "有气体释放。请在通风橱内操作并使用气体捕集装置。";
                case "The mixture changes temperature as the reaction proceeds.": return "反应进行时混合物温度发生变化。";
                case "A visible colour change occurs in the mixture.": return "混合物出现明显颜色变化。";
                case "A chemical transformation is observed in the vessel.": return "反应容器中发生了化学变化。";
                case "Follow the PPE, ventilation and isolation warnings shown by the safety system.": return "请遵循安全系统显示的个人防护、通风和隔离要求。";
                case "Complete. Collect the product and clean the vessel.": return "任务完成。收集产物并清洗反应容器。";
            }

            return value
                .Replace("CURRENT MISSION", "当前任务")
                .Replace("CURRENT OBJECTIVE", "当前目标")
                .Replace("STARTER MISSION", "入门任务")
                .Replace("TOUCH CONTROLS", "触屏操作")
                .Replace("CHEMISTRY SIMULATION · 3D LABORATORY", "化学模拟 · 3D 实验室")
                .Replace("Freely explore chemicals, reaction conditions and laboratory safety.", "自由探索化学品、反应条件与实验室安全。")
                .Replace("Take CuSO₄·5H₂O and NaOH, stage each sample on the vessel tray, then load them to form blue Cu(OH)₂.", "取五水硫酸铜与氢氧化钠，分别放到反应容器旁的托盘上，再装入容器生成蓝色 Cu(OH)₂ 沉淀。")
                .Replace("Left pad — move; push it far to run", "左侧摇杆——移动；推到较远位置自动奔跑")
                .Replace("Swipe the open right side — look around", "在右侧空白区域滑动——转动视角")
                .Replace("INTERACT — pick up / stage / use equipment", "交互——拿取 / 放置 / 使用设备")
                .Replace("INSPECT — view sample or vessel details", "分析——查看样品或反应容器详情")
                .Replace("MISSION — open the step-by-step guide", "任务——打开分步操作指南")
                .Replace("CONDITIONS", "条件")
                .Replace("CATALYST", "催化剂")
                .Replace("OBSERVATION", "现象")
                .Replace("LOCATION", "位置")
                .Replace("CONTENTS", "内容物")
                .Replace("RECORDED INPUTS", "已记录投入物")
                .Replace("CURRENT CONDITIONS", "当前条件")
                .Replace("SAFETY / HANDLING", "安全 / 操作")
                .Replace("HAZARDOUS GAS / VAPOUR", "危险气体 / 蒸气")
                .Replace("STATUS", "状态")
                .Replace("AMOUNT", "用量")
                .Replace("CLASS", "类别")
                .Replace("MOLAR MASS", "摩尔质量")
                .Replace("DENSITY", "密度")
                .Replace("MELTING POINT", "熔点")
                .Replace("BOILING / DECOMPOSITION", "沸点 / 分解")
                .Replace("APPEARANCE", "外观")
                .Replace("SOLUBILITY", "溶解性")
                .Replace("REACTIVITY", "反应性")
                .Replace("HAZARDS", "危险性")
                .Replace("HANDLING", "操作注意")
                .Replace("USE", "用途")
                .Replace("SYNTHESIZED INVENTORY", "合成产物库存")
                .Replace("Picked up ", "已拿取 ")
                .Replace("Returned the held sample.", "已收起手中样品。")
                .Replace("Placed ", "已放置 ")
                .Replace(" on the tray.", " 到托盘上。")
                .Replace("Loaded ", "已装入 ")
                .Replace(" from the tray.", "（来自托盘）。")
                .Replace("Added solvent · volume ", "已加入溶剂 · 体积 ")
                .Replace("Heated · ", "已升温 · ")
                .Replace("Cooled · ", "已降温 · ")
                .Replace("Inventory ", "库存 ")
                .Replace("remaining", "剩余")
                .Replace("purity", "纯度")
                .Replace("batch(es)", "个批次")
                .Replace("Chemical data not found: ", "未找到化学品数据：")
                .Replace("CHEMICAL WARNING · PPE and ventilation may be required.", "化学品警告 · 可能需要个人防护和通风。")
                .Replace("Fume hood fan is on.", "通风橱风机已开启。")
                .Replace("Fume hood fan is off. Hazardous gas will not be captured.", "通风橱风机已关闭，危险气体将无法被有效捕集。")
                .Replace("PPE · ", "防护装备 · ")
                .Replace("GAS TRAP · ", "气体捕集 · ")
                .Replace("AUDIO · ", "声音 · ")
                .Replace("REDUCED MOTION · ", "减少动态效果 · ")
                .Replace("Changes are saved automatically for the next session.", "更改会自动保存，并在下次启动时继续使用。")
                .Replace("PAUSED", "已暂停")
                .Replace("REACTION", "反应")
                .Replace("CLEAN VESSEL", "容器洁净")
                .Replace("REACTION BLOCKED", "反应被阻止")
                .Replace("WAITING FOR REAGENT", "等待试剂")
                .Replace("NO PREDICTED REACTION", "未预测到反应");
        }
    }
}
