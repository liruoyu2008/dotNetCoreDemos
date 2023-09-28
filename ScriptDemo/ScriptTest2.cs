using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ScriptDemo
{
    public class ScriptTest2
    {
        public Func<int> FuncA;
        public Func<int> FuncB;

        public static int A = 0; 

        public void Test()
        {
            var file = @"D:\工作文档\脚本协议\code.txt";

            // 载入
            var code = File.ReadAllText(file);
            // 创建
            var script = CSharpScript.Create(code);
            // 依赖
            var option = ScriptOptions.Default
                .AddReferences(new string[] { "System" })   // 引用
                .AddImports(new string[] { "System", "System.Reflection" });    // 名称空间（也可直接在脚本内写using指令）
            // 编译
            script.Compile();
            // 运行
            CSharpScript.RunAsync(code, option, this, typeof(ScriptTest2));
        }
    }
}
