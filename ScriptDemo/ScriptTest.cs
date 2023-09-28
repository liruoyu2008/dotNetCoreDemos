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
    public class ScriptTest
    {
        public int A = 1;
        public int B = 2;

        public void Test()
        {
            var file0 = @"D:\工作文档\脚本协议\code.txt";
            var file1 = @"D:\code\dc-Ryu\Main\bin\netcoreapp3.1\script_code_pub.txt";
            var file2 = @"D:\code\dc-Ryu\Main\bin\netcoreapp3.1\script_code.txt";

            var code1 = File.ReadAllText(file1);
            var code2 = File.ReadAllText(file2);

            var root1 = CSharpSyntaxTree.ParseText(code1).GetRoot();
            var com1 = root1 as CompilationUnitSyntax;
            var mem1 = com1.Members;

            var root2 = CSharpSyntaxTree.ParseText(code2).GetRoot();
            var com2 = root2 as CompilationUnitSyntax;
            var aaaa = com2.RemoveNodes(root2.DescendantNodes().OfType<UsingDirectiveSyntax>(), SyntaxRemoveOptions.KeepNoTrivia);
            var mem2 = aaaa.Members;

            Console.ReadLine();
        }

        public void Test2()
        {
            Type type = null;
            string str = null;
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
            (type, str) = (ValueTuple<Type, string>)CSharpScript.RunAsync(code, option, this, typeof(ScriptTest)).Result.ReturnValue;

            Console.ReadLine();
        }

        public (string, string) ABC()
        {
            var mtd1 = MethodBase.GetCurrentMethod().Name;
            var cls1 = MethodBase.GetCurrentMethod().DeclaringType.Name;
            return (cls1, mtd1);
        }
    }
}
