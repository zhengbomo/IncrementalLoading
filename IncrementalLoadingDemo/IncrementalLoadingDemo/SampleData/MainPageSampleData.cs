﻿/* ==============================================================================
 * 功能描述：MainPageSampleData  
 * 创 建 者：贤凯
 * 创建日期：1/29/2015 9:34:09 PM
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncrementalLoadingDemo.Helpers;
using IncrementalLoadingDemo.Utility;

namespace IncrementalLoadingDemo.SampleData
{
    public class MainPageSampleData
    {
        public IncrementalLoadingCollection<string, List<string>> Source { get; set; }

        public MainPageSampleData()
        {
            Source = new IncrementalLoadingCollection<string, List<string>>(
                async () =>
                {
                    //加载缓存
                    var list = await CacheManager.GetList();
                    return list;
                }, async result =>
                {
                    //保存缓存
                    await CacheManager.SaveList();
                }, async () =>
                {
                    //刷新请求
                    await Task.Delay(1000);
                    var list = new List<string>();
                    for (int i = 0; i < 50; i++)
                    {
                        //list.Add("刷新数据" + i);
                    }
                    return list;
                }, async pageCount =>
                {
                    //加载请求
                    await Task.Delay(3000);
                    var list = new List<string>();
                    for (int i = 0; i < 50; i++)
                    {
                        list.Add("加载数据" + Guid.NewGuid());
                    }
                    return list;
                }, async (result, items) =>
                {
                    //刷新请求结果  
                    items.Clear();
                    foreach (var r in result)
                    {
                        items.Add(r);
                    }
                    //模拟异步操作（缓存）
                    await Task.Delay(1000);

                    //刷新显示更多
                    items.IsShowEmpty = true;

                    //标识更多
                    items.HasMoreItems = false;

                }, async (result, items) =>
                {
                    //加载请求结果
                    foreach (var r in result)
                    {
                        items.Add(r);
                    }
                    //模拟异步操作
                    await Task.Delay(1000);

                    //标识更多
                    items.HasMoreItems = true;
                }, 10);
        }
    }
}
