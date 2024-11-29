//using microsoft.aspnetcore.mvc;
//using system.collections.generic;
//using system.linq;
//using system.security.claims;
//using system.threading.tasks;
//using tech.areas.admin.models;
//using tech.areas.admin.models.search;
//using tech.service;

//namespace tech.controllers.components

//{
//    //[viewcomponent(name = "categorymenucomponent")]
//    public class productforcategorycomponent : viewcomponent
//    {
//        private readonly iproductsservice _productservice;
//        private readonly icategoryservice _categoryservice;
//        //private readonly iimagesservice _imagesservice;
//        //private readonly iproductsimagesservice _productsimagesservice;
//        public productforcategorycomponent(iproductsservice productservice,
//            icategoryservice categoryservice
//            //iimagesservice imagesservice,
//            //iproductsimagesservice productsimagesservice
//            )
//        {
//            _productservice = productservice;
//            _categoryservice = categoryservice;
//            //_imagesservice = imagesservice;
//            //_productsimagesservice = productsimagesservice;
//        }

//        public async task<iviewcomponentresult> invokeasync()
//        {
//            var categoryserver = _categoryservice.getall();
//            var productforcategory = new list<productforcategorymodelview>();
//            if (categoryserver != null && categoryserver.count > 0)
//            {
//                var productviewmodelsearch = new productviewmodelsearch();
//                productviewmodelsearch.pagesize = 10;
//                productviewmodelsearch.pageindex = 1;
//                foreach (var item in categoryserver)
//                {
//                    var productforcategorymodel = new productforcategorymodelview();
//                    productforcategorymodel.categorymodelview = item;
//                    productviewmodelsearch.categoryid = item.id;
//                    var productmodel = _productservice.getallpaging(productviewmodelsearch);
//                    if (productmodel != null && productmodel.results != null && productmodel.results.count > 0)
//                    {
//                        productforcategorymodel.productmodelviews = productmodel.results.where(p=>p.ishidden != 1).tolist();

//                        foreach (var itemproduct in productforcategorymodel.productmodelviews)
//                        {
//                            if (itemproduct.category_id.hasvalue && itemproduct.category_id.value > 0)
//                            {
//                                var category = _categoryservice.getbyid(itemproduct.category_id.value);
//                                //var productimages = _productsimagesservice.getimageproduct(itemproduct.id);
//                                //if (productimages != null && productimages.count > 0)
//                                //{
//                                //    var lstimages = _imagesservice.getimagename(productimages);
//                                //    if (lstimages != null && lstimages.count > 0)
//                                //    {
//                                //        itemproduct.avatar = lstimages[0].name;
//                                //    }
//                                //}
//                                if (category != null && !string.isnullorempty(category.name))
//                                {
//                                    itemproduct.categorystr = category.name;
//                                }
//                                else
//                                {
//                                    itemproduct.categorystr = "chờ xử lý";
//                                }

//                            }
//                            else
//                            {
//                                itemproduct.categorystr = "";
//                            }
//                            //var productimage = _productsimagesservice.getimageproduct(itemproduct.id);
//                            //if (productimage != null && productimage.count > 0)
//                            //{
//                            //    var image = _imagesservice.getimagename(productimage);
//                            //    if (image != null && image.count > 0)
//                            //    {
//                            //        itemproduct.imagemodelview = image;
//                            //    }
//                            //}
//                            //itemproduct.trademark = !string.isnullorempty(itemproduct.trademark) ? itemproduct.trademark : "";
//                            itemproduct.price_sell_str = itemproduct.price_sell.hasvalue && itemproduct.price_sell.value > 0 ? itemproduct.price_sell.value.tostring("#,###") : "";
//                            itemproduct.price_import_str = itemproduct.price_import.hasvalue && itemproduct.price_import.value > 0 ? itemproduct.price_import.value.tostring("#,###") : "";
//                            itemproduct.price_reduced_str = itemproduct.price_reduced.hasvalue && itemproduct.price_reduced.value > 0 ? itemproduct.price_reduced.value.tostring("#,###") : "";
//                            //item.total_product = 10;

//                        }


//                        productforcategory.add(productforcategorymodel);
//                    }
//                }
//            }                     
//            return view(productforcategory);
//        }
//    }
//}