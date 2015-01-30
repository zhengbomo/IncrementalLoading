/* ==============================================================================
 * 功能描述：IncrementalLoadingCollection  
 * 创 建 者：贤凯
 * 创建日期：1/29/2015 9:21:35 PM
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IncrementalLoadingDemo.Utility
{
    public class IncrementalLoadingCollection<T, TResult> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        //加载缓存
        private readonly Func<Task<IEnumerable<T>>> loadCache;
        private readonly Func<TResult, Task> saveCache;

        //请求
        private readonly Func<Task<TResult>> refreshingRequestAction;
        private readonly Func<int, Task<TResult>> loadingRequestAction;

        //请求结束操作
        private readonly Func<TResult, IncrementalLoadingCollection<T, TResult>, Task> refreshingRequestResultAction;
        private readonly Func<TResult, IncrementalLoadingCollection<T, TResult>, Task> loadingRequestResultAction;

        public int PageCount { get; set; }

        private bool hasMoreItems;
        private bool isLoading;
        private bool isRefreshing;

        /// <summary>
        /// 刷新成功后设置为True
        /// </summary>
        public bool IsShowEmpty { get; set; }

        public bool HasMoreItems
        {
            get { return hasMoreItems; }
            set
            {
                if (hasMoreItems != value)
                {
                    hasMoreItems = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("HasMoreItems"));
                }
            }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    IsLoading = false;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsRefreshing"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsEmpty"));
                }
            }
        }

        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
                }
            }
        }

        public bool IsEmpty
        {
            get { return IsShowEmpty && Count == 0 && !isRefreshing && !isLoading; }
        }

        public IncrementalLoadingCollection(
            Func<Task<IEnumerable<T>>> loadCache,
            Func< TResult, Task> saveCache,
            Func<Task<TResult>> refreshingRequestAction, 
            Func<int, Task<TResult>> loadingRequestAction,
            Func<TResult, IncrementalLoadingCollection<T, TResult>, Task> refreshingRequestResultAction,
            Func<TResult, IncrementalLoadingCollection<T, TResult>, Task> loadingRequestResultAction, int pageCount)
        {
            //加载，保存缓存
            this.loadCache = loadCache;
            this.saveCache = saveCache;
            
            //刷新请求
            this.refreshingRequestAction = refreshingRequestAction;
            //加载请求
            this.loadingRequestAction = loadingRequestAction;
            //刷新请求结果操作
            this.refreshingRequestResultAction = refreshingRequestResultAction;
            //加载请求结果操作
            this.loadingRequestResultAction = loadingRequestResultAction;
            //
            PageCount = pageCount;
        }



        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dispatcher = Window.Current.Dispatcher;

            return AsyncInfo.Run(async cancel =>
            {
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    IsLoading = true;
                });
                var result = await loadingRequestAction.Invoke(PageCount);

                if (!isRefreshing)
                {
                    //防止刷新和加载冲突，当刷新时，不给加载

                    await Task.WhenAll(Task.Delay(10, cancel),
                        dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            //请求结果交给外部处理（添加项，缓存处理等）
                            await loadingRequestResultAction(result, this);

                        }).AsTask(cancel));

                    dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        IsLoading = false;
                    });
                    return new LoadMoreItemsResult {Count = (uint) Count};
                }
                return new LoadMoreItemsResult { Count = (uint)Count };
            });
        }


        public async Task LoadCache()
        {
            var dispatcher = Window.Current.Dispatcher;

            await AsyncInfo.Run(async cancel =>
            {
                var result = await loadCache.Invoke();

                await Task.WhenAll(Task.Delay(10, cancel), dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //请求结果交给外部处理（添加项，缓存处理等）
                    foreach (var i in result)
                    {
                        Add(i);
                    }
                }).AsTask(cancel));
            });
        }

        public async Task RefreshRequest()
        {
            var dispatcher = Window.Current.Dispatcher;

            await AsyncInfo.Run(async cancel =>
            {
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    IsShowEmpty = false;
                    IsRefreshing = true;
                });
                var result = await refreshingRequestAction.Invoke();

                //保存缓存
                await saveCache.Invoke(result);

                await Task.WhenAll(Task.Delay(10, cancel), dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    //请求结果交给外部处理（添加项，缓存处理等）
                    await refreshingRequestResultAction.Invoke(result, this);
                    IsRefreshing = false;
                }).AsTask(cancel));
            });

        }

    }
}
