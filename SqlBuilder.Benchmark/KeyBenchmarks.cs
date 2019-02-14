using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace SqlBuilder.Benchmark
{
  public class KeyBenchmarks
  {
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public void dictionary_with_comparer()
    {
      _dictionaryWithComparer.Add(_key, _now);
      DateTime now = _dictionaryWithComparer[_key];
      _dictionaryWithComparer.Remove(_key);
    }

    [Benchmark]
    public void dictionary_without_comparer()
    {
      _dictionaryWithoutComparer.Add(_key, _now);
      DateTime now = _dictionaryWithoutComparer[_key];
      _dictionaryWithoutComparer.Remove(_key);
    }

    private Dictionary<Key, DateTime> _dictionaryWithComparer = new Dictionary<Key, DateTime>(new KeyComparer());

    private Dictionary<Key, DateTime> _dictionaryWithoutComparer = new Dictionary<Key, DateTime>();

    private readonly Key _key = new Key(typeof(ViewModel), typeof(DataModel));

    private readonly DateTime _now = DateTime.Now;

    private class ViewModel { }

    private class DataModel { }
  }
}