﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class CLR
    {
        public static void Func()
        {
            //
            var fileInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + "//" + AppDomain.CurrentDomain.FriendlyName);
            
        }
    }
    /*
1.如何检测系统是否安装.NET Framework？

　　检查%SystemRoot%\System32目录下是否存在文件MSCorEE.dll。

2.如何确定已安装的.NET Framework版本？

　　HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP

3.关于x86和x64环境：

　　若只有类型安全的托管代码，则在x86和x64都可以正常运行；

　　若含有不安全代码，则需要制定平台和CPU架构；

4.Windows x64为何能运行x86程序？

　　win64提供了WoW64（windows on windows64）技术，该技术模拟x86指令集，但会影响性能；

5.Windows运行Exe过程？

　　a.检测Exe文件头，确定PE32还是PE32+，确定地址空间（32位还是64位或者WoW64），检查CPU架构信息；

　　b.加载MSCorEE.dll对应版本(x86在C:\Windows\System32下，x64的x86版本在C:\Windows\SysWoW64下)；

　　c.使用MSCorEE.dll中的方法初始化CLR，然后加载Exe程序，调用Main方法；

Mark：使用非托管程序加载托管程序集，Windows会自动加载并初始化CLR，但由于进程已经运行，x86托管程序集无法完全加载到64位进程中。

6.执行第一个方法过程：

　　a.在Main函数执行前，CLR检测Main函数内代码引用的所有类型，CLR分配一个内部数据结构，管理所有引用类型的访问；

　　b.在内部数据结构中的每个类型中定义的每个方法都有个一个对应记录项，该项的地址指向该方法的实现，即该项作为一个函数：JITCompiler；

　　c.Main函数内首次调用方法时，该方法对应的JITCompiler函数被调用，该函数将IL代码编译成本地CPU指令，即该组件称为JITter或者JIT编译器；

　　d.JITCompiler在程序集的元数据中查找被调用方法的IL代码，验证IL代码，将IL代码编译为本地CPU指令(CLR的JIT会对本地代码进行优化); 

　　e.CPU指令保存到动态分配的内存块中（程序终止该内存被释放），JITCompiler返回CLR类型创建的内部数据结构，找到被调用方法记录，修改JITCompiler记录项为执行CPU指令的内存块地址；

　　f.JITCompiler跳转到内存块中的代码，执行（此处真正执行函数的CPU指令）并返回Main中；

　　g.第二次调用函数时，会直接执行内存块中的代码，跳过JITCompiler函数，以后对该函数的调用都是本地代码方式进行；

 7.VisualStudio编辑并继续功能的由来：

　　C#编译器使用/optimize-开关在生成的未优化代码中会包含许多NOP（no-operation，空操作）指令和分支指令，利用这些指令进行调试；优化代码则会删除这些多余指令；

8.PDB（Program Database）文件：

　　PDB文化帮助调试器查找局部变量并将IL代码映射到源代码；

9.使用NGen.exe生成本地代码：

　　将一个程序集的所有IL代码编译为本地代码，并保存到一个磁盘文件中，在运行时，加载该程序集前CLR会先判断是否存在这样的预先编译文件；NGen.exe对代码优化保守，不会对代码进行高度优化；

10.IL和验证：

　　IL基于栈，所有指令都要将操作数压栈push，结果从栈中弹出pop；IL指令是无类型的typeless；

　　将IL代码编译为本地代码时，CLR执行验证过程verification，确定代码的安全；托管模块元数据包含验证所需函数和类型；

11.健壮性和可靠性的区别：

　　健壮性：robustness 描述系统对于参数变化的不敏感；可靠性：reliability 描述系统的可靠性，即提供固定的参数，产生稳定的、能预测的输出；

12.PEVerify.exe工具：

　　检查一个程序集的所有方法，并报告其中含有的不安全代码；使用CLR定位依赖的程序集，采用与CLR一样的绑定以及探测规则定位程序集；

13.CLR支持的三种互操作情况：

　　a.托管代码能调用DLL中的非托管代码,采用P/Invoke机制调用DLL中的函数；

　　b.托管代码可以使用现有的COM组件或服务，可以创建一个托管程序集来描述COM组件，可以使用TlbImp.exe工具（将 COM 类型库中的类型定义转换为公共语言运行库程序集中的等效定义）

　　c.非托管代码可以使用托管类型或服务，使用托管代码生成COM组件，TlbExp.exe、RegAsm.exe；

14.MSCorLib.dll - 包含所有核心类型（C#编译器会自动引用该程序集）； 

15.响应文件 response file（.rsp）

　　响应文件是一个包含一组编译器指令开关的文本文件。执行csc.exe时（编译源代码时），编译器打开响应文件使用其中的开关参数,使用@符号引用响应文件;

可以指定多个rsp文件，csc.exe自动隐式查找两个csc.rsp文件：当前目录下；csc.exe目录下（全局rsp文件：%SystemRoot%\Microsoft.NET\Framework\v.XXXXX）；

16.托管PE文件结构：

　　托管PE文件构成：PE32（+）头、CLR头、元数据、IL代码；

PE32（+）头：Windows要求的标准信息；

CLR头：小的信息块（托管模块特有），包含CLR版本号、标志flag、一个MethodDef Token；可选的强名称数字签名；模块内部元数据表的大小和偏移量；

元数据：二进制数据块，3个类别的多个表（源代码中所有定义都会在元数据中的某个表中创建一个记录项，可以使用ILDasm.exe查看元数据）：

定义表（definition table）：ModuleDef、TypeDef、MethodDef、FieldDef、ParaDef、PropertyDef、EventDef等；

引用表（reference table）：AssemblyRef、ModuleRef、TypeRef、MemberRef等；

清单表（manifest table）：AssemblyDef、FileDef、ManifestDef、ExportedTypesDef；

    17.使用程序集连接器 al.exe

    18.强命名程序集
        a.为程序集分配强名称-程序集唯一标识：文件名；版本号；语言文化标识；公钥标记；
        Step-1:使用SN.exe获取一个密钥：
            创建包含二进制私钥和公钥的文件：sn -k <FilePath><Name>.snk；
            创建一个只包含公钥的文件（将snk公钥导出到指定文件）：sn -p <name>.snk <name>.PublicKey;
            显示公钥文件公钥信息：sn -tp <name>.PublicKey;
 
     */
}
