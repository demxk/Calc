using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace bench
{
    public class Bench
    {

        public void normal()
        {
            var s = "";
            for(int i = 0; i < 20; ++i)
                s += i.ToString();

        }
        public virtual void virtual_f()
        {
            var s = "";
            for(int i = 0; i < 20; ++i)
                s += i.ToString();

        }
        public static void static_f()
        {
            var s = "";
            for(int i = 0; i < 20; ++i)
                s += i.ToString();

        }
        public void generic_f<T>()
        {
            var s = "";
            for(int i = 0; i < 20; ++i)
                s += i.ToString();
        }
        [Benchmark(Description = "normal function")]
        public void Normal()
        {
            normal();
        }

        [Benchmark(Description = "virtual function")]
        public void Virtual()
        {
            virtual_f();
        }

        [Benchmark(Description = "static function")]
        public void Static()
        {
            static_f();
        }
        [Benchmark(Description = "generic fn")]
        public void Generic()
        {
            generic_f<int>();
        }
        [Benchmark(Description = "generic 2 fn")]
        public void Generic2()
        {
            generic_f<string>();
        }

        public void dynamic_f()
        {
            dynamic s = "";
            for(int i = 0; i < 20; ++i)
                s += i.ToString();
        }

        [Benchmark(Description = "dynamic fn")]
        public void Dynamic()
        {
            dynamic_f();
        }
        
        public void reflection_f()
        {
            GetType().GetMethod("normal").Invoke(this, null);

        }
        [Benchmark(Description = "reflection fn")]
        public void Reflection()
        {
            reflection_f();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Bench>();
        }
    }
}
