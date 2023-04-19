using IntusWindowsInterview.Common;
using IntusWindowsInterview.Common.Configuration;
using IntusWindowsInterview.Model;
using IntusWindowsInterview.Model.CommonModel;
using IntusWindowsInterview.Model.DBModel;
using IntusWindowsInterview.Model.ViewModel;
using IntusWindowsInterview.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace IntusWindowsInterview.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ConnectionStringConfig _connectionStringConfig;
        public OrderServices(UnitOfWork unitOfWork, ConnectionStringConfig connectionStringConfig)
        {
            _unitOfWork = unitOfWork;
            _connectionStringConfig = connectionStringConfig;
        }
        public async Task<ServiceResponse<OrderViewModel>> GetOrderById(long Id)
        {
            OrderViewModel RequestedOrder = new OrderViewModel();
            try
            {
                var spName = EnumObjects.IntusWindowsInterview.IntusWindowsREST_GetOrder.ToString();
                var commandParameterList = new List<SqlCommandParameter>();
                commandParameterList.Add(SqlCommandParameter.AddParameter("@Id", (Convert.ToInt64(Id)).ToDBNullIfNothing()));

                foreach (var item in commandParameterList.Cast<SqlCommandParameter>().ToList())
                {
                    if (item.ParameterValue == DBNull.Value || item.ParameterValue == "" || item.ParameterValue == null)
                    {
                        commandParameterList.Remove(item);
                    }
                }

                var result_dt = await DataHelper.ExecuteStoredProcedure<List<OrderSerializeViewModel>>(spName, commandParameterList, _connectionStringConfig.DefaultConnection);

                var result = JsonConvert.DeserializeObject<List<OrderSerializeViewModel>>(JsonConvert.SerializeObject(result_dt));

                if (result.Count() == 0)
                {
                    return ServiceResponse<OrderViewModel>.NotFound();
                }

                var subElementList = result.GroupBy(x => x.SubElementId).FirstOrDefault().Select(x => new SubElementViewModel
                {
                    Id = x.SubElementId,
                    Element = x.Element,
                    Height = x.Height,
                    Type = x.Type,
                    Width = x.Width,
                    WindowId = x.WindowId
                }).ToList();

                var windowList = result.GroupBy(x => x.WindowId).FirstOrDefault().Select(x => new WindowViewModel
                {
                    Id = x.WindowId,
                    Name = x.WindowName,
                    OrderId = x.OrderId,
                    QuantityOfWindows = x.QuantityOfWindows,
                    TotalSubElements = x.TotalSubElements,
                    SubElementsViewModel = subElementList.Where(y => y.WindowId == x.WindowId).ToList()
                }).ToList();

                var order = result.GroupBy(x => x.OrderId).Select(x => new OrderViewModel()
                {
                    Id = x.Key,
                    Name = x.FirstOrDefault().OrderName,
                    State = x.FirstOrDefault().State,
                    WindowsViewModels = windowList.Where(y => y.OrderId == x.FirstOrDefault().OrderId).ToList()
                }).FirstOrDefault();

                RequestedOrder = order;

                return new ServiceResponse<OrderViewModel>
                {
                    data = RequestedOrder,
                    message = new List<string>() { "order retrieved successfully." },
                    success = true
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse<OrderViewModel>
                {
                    data = null,
                    message = new List<string>() { ex.Message },
                    success = false
                };
            }

            
        }

        public async Task<ServiceResponse<List<OrderViewModel>>> GetOrders()
        {
            List< OrderViewModel> RequestedOrder = new List<OrderViewModel>();
            try
            {
                var spName = EnumObjects.IntusWindowsInterview.IntusWindowsREST_GetOrder.ToString();
                var commandParameterList = new List<SqlCommandParameter>();

                foreach (var item in commandParameterList.Cast<SqlCommandParameter>().ToList())
                {
                    if (item.ParameterValue == DBNull.Value || item.ParameterValue == "" || item.ParameterValue == null)
                    {
                        commandParameterList.Remove(item);
                    }
                }

                var result_dt = await DataHelper.ExecuteStoredProcedure<List<OrderSerializeViewModel>>(spName, commandParameterList, _connectionStringConfig.DefaultConnection);

                var result = JsonConvert.DeserializeObject<List<OrderSerializeViewModel>>(JsonConvert.SerializeObject(result_dt));

                if (result.Count() == 0)
                {
                    return new ServiceResponse<List<OrderViewModel>>
                    {
                        data = null,
                        message = new List<string>() { "order list retrieved successfully." },
                        success = false
                    };
                }

                var subElementList = result.GroupBy(x => x.SubElementId).FirstOrDefault().Select(x => new SubElementViewModel
                {
                    Id = x.SubElementId,
                    Element = x.Element,
                    Height = x.Height,
                    Type = x.Type,
                    Width = x.Width,
                    WindowId = x.WindowId
                }).ToList();

                var windowList = result.GroupBy(x => x.WindowId).FirstOrDefault().Select(x => new WindowViewModel
                {
                    Id = x.WindowId,
                    Name = x.WindowName,
                    OrderId = x.OrderId,
                    QuantityOfWindows = x.QuantityOfWindows,
                    TotalSubElements = x.TotalSubElements,
                    SubElementsViewModel = subElementList.Where(y => y.WindowId == x.WindowId).ToList()
                }).ToList();

                var order = result.GroupBy(x => x.OrderId).Select(x => new OrderViewModel()
                {
                    Id = x.Key,
                    Name = x.FirstOrDefault().OrderName,
                    State = x.FirstOrDefault().State,
                    WindowsViewModels = windowList.Where(y => y.OrderId == x.FirstOrDefault().OrderId).ToList()
                }).ToList();

                RequestedOrder = order;

                return new ServiceResponse<List<OrderViewModel>>
                {
                    data = RequestedOrder,
                    message = new List<string>() { "order retrieved successfully." },
                    success = true
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse<List<OrderViewModel>>
                {
                    data = null,
                    message = new List<string>() { ex.Message },
                    success = false
                };
            }
        }

        public async Task<ServiceResponse<OrderViewModel>> CreateOrder(OrderViewModel order)
        {
            try
            {
                OrderViewModel responseViewModel = new OrderViewModel();

                Order createOrder = new Order
                {
                    Name = order.Name,
                    State = order.State,
                    Windows = order.WindowsViewModels.Select(y => new Window()
                              {
                                 Name = y.Name,
                                 QuantityOfWindows = y.QuantityOfWindows,
                                 TotalSubElements = y.TotalSubElements,
                                 SubElements = y.SubElementsViewModel.Select(z=> new SubElement()
                                               {
                                                    Element = z.Element,
                                                    Height = z.Height,
                                                    Type = z.Type,
                                                    Width = z.Width
                                               }).ToList()
                              }).ToList()
                };
                await _unitOfWork.Orders.AddAsync(createOrder);
                await _unitOfWork.SaveAsync();

                responseViewModel = new OrderViewModel()
                {
                    Id = createOrder.Id,
                    Name= createOrder.Name,
                    State = createOrder.State,
                    WindowsViewModels = createOrder.Windows.Select(x=> new WindowViewModel()
                    {
                        Id= x.Id,
                        Name = x.Name,
                        OrderId= x.OrderId,
                        QuantityOfWindows= x.QuantityOfWindows,
                        TotalSubElements= x.TotalSubElements,
                        SubElementsViewModel = x.SubElements.Select(y => new SubElementViewModel()
                                                                    { 
                                                                        Id = y.Id,
                                                                        Element = y.Element,
                                                                        Height = y.Height,
                                                                        Type = y.Type,
                                                                        Width = y.Width,
                                                                        WindowId = y.WindowId
                                                                    }).ToList()
                    }).ToList()
                };

                return ServiceResponse<OrderViewModel>.AddedSuccessfully(responseViewModel);


            }
            catch(Exception ex)
            {
                return new ServiceResponse<OrderViewModel>
                {
                    data = order,
                    message = new List<string>() { ex.Message },
                    success = false
                };
            };
        }

        public async Task<ServiceResponse<OrderViewModel>> UpdateOrder(long Id, OrderViewModel updateOrder)
        {
            try
            {
                var order = _unitOfWork.Orders.Get(x=> x.Id == Id).FirstOrDefault();

                if (order == null)
                {
                    return ServiceResponse<OrderViewModel>.NotFound();
                }

                updateOrder.Id = Id;

                var editedWindows = updateOrder.WindowsViewModels.Where(x => x.Id > 0).Select(y => new Window()
                {
                    Id = order.Id,
                    Name = y.Name,
                    QuantityOfWindows = y.QuantityOfWindows,
                    TotalSubElements = y.TotalSubElements,
                    SubElements = y.SubElementsViewModel.Select(z => new SubElement
                    {
                        Id = order.Id,
                        Type = z.Type,
                        Element = z.Element,
                        Height = z.Height,
                        Width = z.Width
                    }).ToList()
                }).ToList();

                order.Windows = editedWindows;

                var newAddedWindows = updateOrder.WindowsViewModels.Where(x => x.Id == 0).Select(y => new Window()
                                                                                                {   
                                                                                                    OrderId = order.Id,
                                                                                                    Name = y.Name,
                                                                                                    QuantityOfWindows = y.QuantityOfWindows,
                                                                                                    TotalSubElements = y.TotalSubElements,
                                                                                                    SubElements = y.SubElementsViewModel.Select(z=> new SubElement
                                                                                                                                        {
                                                                                                                                            Type = z.Type,
                                                                                                                                            Element = z.Element,
                                                                                                                                            Height =z.Height,
                                                                                                                                            Width =z.Width
                                                                                                                                        }).ToList()
                                                                                                }).ToList();

                order.Windows.AddRange(newAddedWindows);

                _unitOfWork.Orders.Update(order);

                await _unitOfWork.SaveAsync();

                return ServiceResponse<OrderViewModel>.UpdatedSuccessfully(updateOrder);

            }
            catch (Exception ex)
            {
                return new ServiceResponse<OrderViewModel>
                {
                    data = updateOrder,
                    message = new List<string>() { ex.Message },
                    success = false
                };
            }
        }

        public async Task<ServiceResponse<OrderViewModel>> DeleteOrder(long Id)
        {
            var getOrder = await GetOrderById(Id);
            if(!getOrder.success)
            {
                return ServiceResponse<OrderViewModel>.Error("order not found");
            }

            try
            {
                _unitOfWork.Orders.Delete(Id);
                await _unitOfWork.SaveAsync();

                return ServiceResponse<OrderViewModel>.Success("Successfully Deleted");
            }
            catch(Exception ex)
            {
                return new ServiceResponse<OrderViewModel>
                {
                    data = null,
                    message = new List<string>() { ex.Message },
                    success = false
                };
            }
        }
    }

    public interface IOrderServices
    {
        Task<ServiceResponse<OrderViewModel>> GetOrderById(long Id);
        Task<ServiceResponse<List<OrderViewModel>>> GetOrders();
        Task<ServiceResponse<OrderViewModel>> CreateOrder(OrderViewModel order);
        Task<ServiceResponse<OrderViewModel>> UpdateOrder(long Id, OrderViewModel order);
        Task<ServiceResponse<OrderViewModel>> DeleteOrder(long Id);
    }
}