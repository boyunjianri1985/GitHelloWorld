﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.DesignPattern
{
    #region Action Pattern

    /// <summary>
    /// 责任链模式
    /// 某个请求需要多个对象对其进行处理，避免请求发送者和接收之间的耦合关系
    /// 处理方式，将对象连城一个链，并沿着该链进行传递该请求，直到有对象处理为止
    /// 需要：
    /// 1.抽象请求处理者-定义处理请求的接口，内含自身的单向链表，包含下一个具体处理者引用
    /// 2.具体请求处理者-接收到请求后，进行判断处理，然后交给下一个处理者或者直接输出结果
    /// </summary>
    public class ChainOfResponsibilityPattern
    {

        public abstract class Handler
        {
            public Handler NextHandler { get; set; }
            public abstract void Process(Request request);

            protected void CallBack(Request request)
            {
                if (NextHandler != null)
                {
                    NextHandler.Process(request);
                }
            }
        }

        public class Request
        {
            public int Level { get; set; }
        }

        public class ConcreteHandler1 : Handler
        {
            public override void Process(Request request)
            {
                if (request.Level <= 1)
                {
                    Console.WriteLine("Porcess Request::ConcreteHandler1");
                }
                else
                {
                    CallBack(request);
                }
            }
        }
        public class ConcreteHandler2 : Handler
        {
            public override void Process(Request request)
            {
                if (request.Level == 2)
                {
                    Console.WriteLine("Porcess Request::ConcreteHandler2");
                }
                else
                {
                    CallBack(request);
                }
            }
        }
        public class ConcreteHandler3 : Handler
        {
            public override void Process(Request request)
            {
                if (request.Level == 3)
                {
                    Console.WriteLine("Porcess Request::ConcreteHandler3");
                }
                else
                {
                    CallBack(request);
                }
            }
        }

        public static void Test_ChainOfResponsibility()
        {
            var handle1 = new ConcreteHandler1();
            var handle2 = new ConcreteHandler2();
            var handle3 = new ConcreteHandler3();
            handle1.NextHandler = handle2;
            handle2.NextHandler = handle3;

            handle1.Process(new Request());


        }


    }

    /// <summary>
    /// 命令模式
    /// 高内聚的模式
    /// 将一个请求封装成一个对象
    /// 解决命令的请求者和命令的实现者之间的耦合
    /// 需要：
    /// 1.抽象命令-cmd
    /// 2.具体命令-ccmd
    /// 3.抽象执行者-invoker
    /// 4.具体执行者-cinvoker 执行具体的命令
    /// 5.抽象接收者-receiver
    /// 6.具体接收者-creceiver 接收命令，分派命令到之下者；
    /// </summary>
    public class CommandPattern
    {
        /// <summary>
        /// 抽象命令
        /// </summary>
        public abstract class Command
        {
            public string Cmd { get; set; }
            protected Command(string str)
            {
                Cmd = str;
            }
            public abstract void Execute();
            public abstract void Undo();
        }

        public class Select : Command
        {
            public Select(string str) : base(str)
            {

            }

            public override void Execute()
            {
                throw new NotImplementedException();
            }

            public override void Undo()
            {
                throw new NotImplementedException();
            }
        }
        public class Update : Command
        {
            public Update(string str) : base(str)
            {

            }

            public override void Execute()
            {
                throw new NotImplementedException();
            }

            public override void Undo()
            {
                throw new NotImplementedException();
            }
        }

        public abstract class Invoker
        {
            public abstract void Execute(Command cmd);
        }

        public class ConcreteInvoker1 : Invoker
        {
            public override void Execute(Command cmd)
            {
                Console.WriteLine(this.GetType().ToString() + "::" + cmd.ToString());
            }
        }

        public class ConcreteInvoker2 : Invoker
        {
            public override void Execute(Command cmd)
            {
                Console.WriteLine(this.GetType().ToString() + "::" + cmd.ToString());
            }
        }

        public abstract class Receiver
        {
            private Invoker _invoker;
            public Receiver(Invoker invoker)
            {
                _invoker = invoker;
            }

            public abstract void ReceiveCmd(Command cmd);
        }

        public class Receiver01 : Receiver
        {
            public Receiver01(Invoker invoker) : base(invoker)
            {

            }
            public override void ReceiveCmd(Command cmd)
            {
            }
        }

        public class Receiver02 : Receiver
        {
            public Receiver02(Invoker invoker) : base(invoker)
            {

            }
            public override void ReceiveCmd(Command cmd)
            {
            }
        }

        public static void Test_Command()
        {

        }

    }

    /// <summary>
    /// 解释器模式
    /// </summary>
    public class InterpreterPattern
    {
        public abstract class Expression
        {
            public abstract void Interpret(Context context);
        }

        public class TerminalExpression : Expression
        {
            public override void Interpret(Context context)
            {
                var t = context.Operators.ToList().Find(s => s.Key == context.Input);
                int result = 0;
                if (t.Value != null)
                {
                    result = t.Value.Invoke(context.A, context.B);
                }
                Console.WriteLine("Terminal Expression:" + result);
            }
        }

        public class NoneterminalExpression : Expression
        {
            public override void Interpret(Context context)
            {
                var t = context.Operators.ToList().Find(s => s.Key == context.Input);
                int result = 0;
                if (t.Value != null)
                {
                    result = t.Value.Invoke(context.A, context.B);
                }
                Console.WriteLine("None Terminal Expression:" + result);
            }
        }

        public class Context
        {
            public Dictionary<char, Func<int, int, int>> Operators = new Dictionary<char, Func<int, int, int>>
            {
                {'+',(int a,int b)=> {return a+b; } },
                {'-',(int a,int b)=> {return a-b; } },
                {'*',(int a,int b)=> {return a*b; } },
                {'/',(int a,int b)=> {if(b!=0)return a/b;else return 0; } },
                {'%',(int a,int b)=> {if(b!=0)return a%b;else return 0; } },
                {'|',(int a,int b)=> {return a|b; } },
                {'^',(int a,int b)=> {return a^b; } },
            };

            public int A { get; set; }
            public int B { get; set; }

            public char Input { get; set; }

        }



    }

    /// <summary>
    /// 迭代器模式
    /// </summary>
    public class IteratorPattern
    {
        public interface Iterator
        {

        }

        public class ConcreteIterator1 : Iterator
        {
            private IList<object> _list;
            private int _index = 0;

            public object Current
            {
                get
                {
                    return _list[_index];
                }
            }

            public bool MoveNext()
            {
                if (_list.Count >= _index + 1)
                {
                    ++_index;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = 0;
            }
        }

        public class ConcreteIterator2 : Iterator
        {
            private object[] _list;
            private int _index = 0;

            public object Current
            {
                get
                {
                    return _list[_index];
                }
            }

            public bool MoveNext()
            {
                if (_list.Length >= _index + 1)
                {
                    ++_index;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = 0;
            }
        }

        public abstract class Aggregate
        {
            public abstract Iterator CreateIterator();
        }

        public class ConcreteAggregate1 : Aggregate
        {
            public override Iterator CreateIterator()
            {
                return new ConcreteIterator1();
            }
        }
        public class ConcreteAggregate2 : Aggregate
        {
            public override Iterator CreateIterator()
            {
                return new ConcreteIterator2();
            }
        }
    }

    /// <summary>
    /// 中介者模式
    /// </summary>
    public class MediatorPattern
    {
        public abstract class Mediator
        {
            public IList<Colleage> colls = new List<Colleage>();
            public abstract void Contact(Message msg, Colleage coll);
        }

        public class Message
        {
            public string Msg { get; set; }
            public override string ToString()
            {
                return Msg;
            }
        }

        public class ConcreteMediator1 : Mediator
        {
            public override void Contact(Message msg, Colleage coll)
            {
                //TODO:get target colleage and call receive function
                colls.ToList().ForEach(c => { if (c != coll) { c.ReceiveMsg(msg); } });
            }
        }

        public abstract class Colleage
        {
            private Mediator _med;
            public Mediator Med { get { return _med; } set { value.colls.Add(this); _med = value; } }
            public string ID { get; set; }
            public abstract void ReceiveMsg(Message msg);
            public virtual void SendMsg(Message msg)
            {
                Med.Contact(msg, this);
            }
        }

        public class ConcreteColleage1 : Colleage
        {
            public override void ReceiveMsg(Message msg)
            {
                Console.WriteLine(this.GetType().ToString() + "::" + msg);
            }

            public override void SendMsg(Message msg)
            {
                base.SendMsg(msg);
            }
        }

        public class ConcreteColleage2 : Colleage
        {
            public override void ReceiveMsg(Message msg)
            {
                Console.WriteLine(this.GetType().ToString() + "::" + msg);
            }
            public override void SendMsg(Message msg)
            {
                base.SendMsg(msg);
            }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class MementoPattern
    {
        public class Memento
        {
            private string _state { get; set; }
            public void SetState(string state)
            {
                _state = state;
            }

            public string GetState()
            {
                return _state;
            }
        }

        public class CareTaker
        {
            private Memento _memen { get; set; }

            public Memento GetMemento()
            {
                return _memen;
            }

            public void SetMemento(Memento memento)
            {
                _memen = memento;
            }

        }
        public class Originator
        {
            public Memento SaveMemento()
            {
                return new Memento();
            }
            public void RestoreMemento(Memento memento)
            {

            }
        }



    }

    /// <summary>
    /// 观察者模式
    /// </summary>
    public class ObserverPattern
    {
        public interface IObserver
        {
            void Update(string msg);
        }

        public class ConcreteObserver1 : IObserver
        {
            public void Update(string msg)
            {
                throw new NotImplementedException();
            }
        }

        public class ConcreteObserver2 : IObserver
        {
            public void Update(string msg)
            {
                throw new NotImplementedException();
            }
        }

        public interface ISubject
        {
            IList<IObserver> Observers { get; set; }
            void Add(IObserver ser);
            void Renmove(IObserver ser);
            void Notify(string msg);
        }

        public class ConcreteSubject : ISubject
        {
            public IList<IObserver> Observers { get; set; } = new List<IObserver>();

            public void Add(IObserver ser)
            {
                Observers.Add(ser);
            }

            public void Notify(string msg)
            {
                Observers.ToList().ForEach(s => s.Update(msg));
            }

            public void Renmove(IObserver ser)
            {
                Observers.Remove(ser);
            }
        }



    }

    /// <summary>
    /// 状态模式
    /// </summary>
    public class StatePattern
    {
        public class Context
        {
            private State _state;
            public void SetState(State state)
            {
                //set state change and set action
                state.Handle();
            }
        }

        public abstract class State
        {
            public abstract void Handle();
        }

        public class ConcreteState1 : State
        {
            public override void Handle()
            {
                throw new NotImplementedException();
            }
        }

        public class ConcreteState2 : State
        {
            public override void Handle()
            {
                throw new NotImplementedException();
            }
        }


    }

    /// <summary>
    /// 策略模式
    /// </summary>
    public class StrategyPattern
    {
        public class Context
        {
            private Strategy _strategy;

            public void SetStrategy(Strategy st)
            {
                _strategy = st;
            }

            public void RunAlgorithm()
            {
                _strategy?.Algorithm();
            }
        }

        public abstract class Strategy
        {
            protected IList<IComparable> _array;
            public int CalculateDegree()
            {
                return 0;
            }
            public abstract void Algorithm();
        }

        public class ConcreteStrategy1 : Strategy
        {
            /// <summary>
            /// bubble sort
            /// </summary>
            public override void Algorithm()
            {
                for (int i = 0; i < _array.Count; i++)
                {
                    for (int j = i + 1; j < _array.Count; j++)
                    {
                        if (_array[i].CompareTo(_array[j]) > 0)
                        {
                            var temp = _array[j];
                            _array[j] = _array[i];
                            _array[i] = temp;
                        }
                    }
                }
            }
        }

        public class ConcreteStrategy2 : Strategy
        {
            /// <summary>
            /// quick sort
            /// </summary>
            public override void Algorithm()
            {
                Sort_q(_array, 0, _array.Count - 1);
            }

            private void Sort_q(IList<IComparable> rest, int left, int right)
            {
                if (left < right)
                {
                    //找到中间元素为中间值
                    var middle = rest[(left + right) / 2];
                    int i = left - 1,
                        j = right + 1;
                    while (true)
                    {
                        //找到比middle小的元素
                        while (rest[++i].CompareTo(middle) < 0 && i < right) ;
                        //找到比middle大的元素
                        while (rest[--j].CompareTo(middle) > 0 && j > 0) ;
                        //若越界则退出循环
                        if (i >= j)
                            break;
                        //交互元素
                        var num = rest[i];
                        rest[j] = rest[j];
                        rest[j] = num;
                    }
                    //迭代排序
                    Sort_q(rest, left, i - 1);
                    Sort_q(rest, j + 1, right);
                }
            }
        }

        public class ConcreteStrategy3 : Strategy
        {
            /// <summary>
            /// insert sort
            /// </summary>
            public override void Algorithm()
            {
                for (int i = 1; i < _array.Count; i++)
                {
                    int j;
                    var temp = _array[i];//index i
                    for (j = i; j > 0; j--)//遍历i之前元素
                    {
                        if (_array[j - 1].CompareTo(temp) > 0)//比较当前元素与之前元素
                        {
                            _array[j] = _array[j - 1];
                        }
                        else
                        {
                            break;
                        }
                    }
                    _array[j] = temp;
                }
            }
        }

        public class ConcreteStrategy4 : Strategy
        {
            /// <summary>
            /// select sort
            /// </summary>
            public override void Algorithm()
            {
                IComparable temp;
                for (int i = 0; i < _array.Count; i++)
                {
                    temp = _array[i];//index i
                    int j;
                    int select = i;
                    for (j = i + 1; j < _array.Count; j++)
                    {
                        if (_array[j].CompareTo(temp) < 0)
                        {
                            temp = _array[j];
                            select = j;
                        }
                    }
                    _array[select] = _array[i];
                    _array[i] = temp;
                }
            }
        }


    }

    /// <summary>
    /// 模板模式
    /// 定义一个模板类，预留使用的每个抽象实现
    /// 子类实现每个抽象实现
    /// </summary>
    public class TemplateMethodPattern
    {
        public abstract class Abstraction
        {
            public void TemplateMethod()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Func1();
                Func2();
                Func3();
            }
            protected abstract void Func1();
            protected abstract void Func2();
            protected abstract void Func3();
        }

        public class ConcreteClass1 : Abstraction
        {
            protected override void Func1()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            protected override void Func2()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            protected override void Func3()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public class ConcreteClass2 : Abstraction
        {
            protected override void Func1()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

            protected override void Func2()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

            protected override void Func3()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }


    }

    /// <summary>
    /// 访问者模式
    /// 封装某些作用于某种数据结构的操作
    /// 用于在不改变数据结构的情况下定义作用于元素的操作
    /// </summary>
    public class VisitorPattern
    {
        /// <summary>
        /// 抽象访问者
        /// </summary>
        public abstract class Visitor
        {
            /// <summary>
            /// 指定访问的元素
            /// 具体访问元素的方法
            /// </summary>
            /// <param name="element"></param>
            public abstract void Visit(Element element);
        }

        /// <summary>
        /// 抽象元素
        /// </summary>
        public abstract class Element
        {
            /// <summary>
            /// 指定接收的访问者，对访问者进行过滤
            /// </summary>
            /// <param name="vistor"></param>
            public abstract void Accept(Visitor vistor);
            /// <summary>
            /// 元素内方法
            /// </summary>
            public abstract void Func();
        }

        /// <summary>
        /// 具体访问者
        /// </summary>
        public class ConcreteVisotor1 : Visitor
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            public override void Visit(Element e)
            {
                e.Func();
            }
        }



        public class ConcreteElement1 : Element
        {
            public override void Accept(Visitor vistor)
            {
                vistor.Visit(this);
            }

            public override void Func()
            {
                //Console.WriteLine(this.GetType().ToString() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// 具体元素集合
        /// </summary>
        public class ObjectStruct
        {
            public IList<Element> Elements = new List<Element>();
            public void AddElement(Element ele)
            {
                Elements.Add(ele);
            }

            public void RemoveElement(Element ele)
            {
                Elements.Remove(ele);
            }

            public void Accept(Visitor vistor)
            {
                Elements.ToList().ForEach(e => e.Accept(vistor));
            }
        }



    }

    #endregion
}
