var INS = {
	init: function() {
		this.Main.init();
		if(window.shop.template == 'index' ){
			this.Home.init();
		}
		if(window.shop.template == 'product' ){
			this.Product.init();
		}
		if(window.shop.template == 'collection' ){
			this.Collection.init();
		}
	},
	resize: function() {
		this.actionResize.init();
	},
	load: function() {
		this.loadPage.init();
	}
}
$(document).ready(function() {
	INS.init();
})
$(window).on('load resize', function(){
	INS.resize();
})
$(window).on('resize', function(){
	if(window.shop.template == 'index' ){
		INS.loadPage.responsiveHeading();
	}
})
$(window).load(function(){
	INS.load();
})
/* jQuery for all*/
INS.Main = {
	init: function(){
		this.fastClick();
		this.searchAutoComplete();
		this.scrollToTop();
		this.productItemAddCart();
		this.replaceImageLoop();
		this.scrollReplaceRegExpImg();
		this.mobileActions();
	},
	fastClick: function(){
		$(function() {
			FastClick.attach(document.body);
		});
	},
	searchAutoComplete: function(){
		var $input = $('#searchFRM input[type="text"]');
		$input.bind('keyup change paste propertychange', function() {
			var key = $(this).val(),
					$parent = $(this).parents('.frmSearch'),
					$results = $(this).parents('.frmSearch').find('.ajaxSearchAuto');
			if(key.length > 0 && key !== $(this).attr('data-history')){
				$(this).attr('data-history', key);
				var str = '/search?type=product&q=' + key + '&view=auto-complete';
				$.ajax({
					url: str,
					type: 'GET',
					async: true,
					success: function(data){
						$results.find('.resultsContent').html(data);
					}
				})
				$results.fadeIn();
			}
		})
		$('body').click(function(evt) {
			var target = evt.target;
			if (target.id !== 'ajaxSearchResults' && target.id !== 'inputSearchAuto') {
				$("#ajaxSearchResults").hide();
			}
		});
		$('body').on('click', '#searchFRM input[type="text"]', function() {
			if ($(this).is(":focus")) {
				if ($(this).val() != '') {
					$("#ajaxSearchResults").show();
				}
			} else {

			}
		})
	},
	scrollToTop: function() {
		jQuery(window).scroll(function() {
			if ($(this).scrollTop() > 100) {
				$('.tempFixed .itemFixed').addClass('trans');
				$('.headerTemp').addClass('headerFixed');
			} else {
				$('.tempFixed .itemFixed').removeClass('trans');
				$('.headerTemp').removeClass('headerFixed');
			}
		});

		//Click event to scroll to top
		jQuery('.backTop a').click(function() {
			$('html, body').animate({
				scrollTop: 0
			}, 600);
			return false;
		});
		jQuery('.tempFixed .itemFixed.hotLine label i').click(function() {
			$(this).parents('.hotLine').fadeOut(500)
		});
	},
	productItemAddCart: function(){
		$(document).on('click','.Addcart', function(){
			var qty = 1,
					variantID = $(this).attr('data-variantid'),
					cart = ($('.headerTemp').hasClass('headerFixed')) ? $('.tempFixed .itemFixed.cartFixed') : $('#headerUser .wrap .cartBox .icon'),
					image = $(this).parents('.pdLoopItem').find('.pdLoopImg img').eq(0);
			cart.addClass('loading');
			INS.Main.flyToElement(image,cart);
			INS.Main.ajaxAddCart(qty,variantID);
		});
		$(document).on('click','#quick-view-modal .btn-addcart', function(e){
			e.preventDefault();
			var qty = parseInt($('#quick-view-modal .form-input input[type=number]').val()),
					variantID = $('#quick-view-modal select#p-select').val(),
					cart = ($('.headerTemp').hasClass('headerFixed')) ? $('.tempFixed .itemFixed.cartFixed') : $('#headerUser .wrap .cartBox .icon'),
					image = $(this).parents('#quick-view-modal').find('.p-product-image-feature');
			cart.addClass('loading');
			INS.Main.flyToElement(image,cart);
			INS.Main.ajaxAddCart(qty,variantID);
		})
	},
	flyToElement(flyer, flyingTo) {
		var $func = $(this);
		var divider = 10;
		var flyerClone = $(flyer).clone().css('width','100px');
		$(flyerClone).css({position: 'absolute', top: $(flyer).offset().top + "px", left: $(flyer).offset().left + "px", opacity: 1, 'z-index': 100001});
		$('body').append($(flyerClone));
		var gotoX = $(flyingTo).offset().left + ($(flyingTo).width() / 2) - ($(flyer).width()/divider)/2;
		var gotoY = $(flyingTo).offset().top + ($(flyingTo).height() / 2) - ($(flyer).height()/divider)/2;

		$(flyerClone).animate({
			opacity: 0.5,
			left: gotoX,
			top: gotoY,
			width: 10,
			height: 10
		}, 1000,
													function () {
														$(flyingTo).fadeOut('fast', function () {
															$(flyingTo).fadeIn('fast', function () {
																$(flyerClone).fadeOut('fast', function () {
																	$(flyerClone).remove();
																	flyingTo.removeClass('loading');
																});
															});
														});
													});
	},
	ajaxAddCart: function(qty,id){
		var cartItem = parseInt($('#cartCount').text());
		var params = {
			type: 'POST',
			url: '/cart/add.js',
			data: 'quantity=' + qty + '&id=' + id,
			dataType: 'json',
			success: function(line_item) { 
				$('.numberCart').text(cartItem + qty).removeClass('hide');
				INS.Main.updateMoneyCart();
			},
			error: function(XMLHttpRequest, textStatus) {
				Haravan.onError(XMLHttpRequest, textStatus);
			}
		};
		jQuery.ajax(params);
	},
	updateMoneyCart: function(){
		Haravan.getCart(function(cart){
			var totalPrice = Haravan.formatMoney(cart.total_price, window.shop.moneyFomat);
			$('#cartPrice').text(totalPrice);
		})
	},
	replaceImageLoop: function(){
		var imgArr = ['_thumb','_compact','_medium','_large'],
				time = 1500,
				index = 0,
				key = '_icon';
		var timeReplace = setInterval(function(){
			$('.imgLoopItem').each(function(){
				var imgSrc = $(this).attr('src'),
						imgReg = imgSrc.replace(key,imgArr[index]);
				$(this).attr('src',imgReg);
			})
			key = imgArr[index];
			index++;
			if( index >= imgArr.length ){
				$('.imgLoopItem').attr('data-reg',true).css('width','auto');
				clearInterval(timeReplace);
				return;
			}
		},time);
	},
	scrollReplaceRegExpImg: function(){
		var $imgEl = $('.imgLoopItem');
		$(window).scroll(function(){
			var sizeReplace = $('.imgLoopItem[data-reg="false"]').length;
			if(sizeReplace == 0){
				$imgEl.css('width','auto');
				return;
			}
			$imgEl.each(function(index, el){
				var src = $(el).attr('src'),
						regKey = /icon|thumb|compact|medium|large/gi,
						check = regKey.test(src);
				if(check){
					var regExp = src.replace(regKey, "large");
					$(el).attr({'data-reg': true, 'src': regExp});
				} 
			});
		})
	},
	mobileActions: function(){
		$(document).on('click','.btnMBToggleNav, .closeMenuMB, .overlayMenu', function(){
			$('body').toggleClass('openNav');
		});
		$(document).on('click','.navSiteMain ul.nav-navbar li a i', function(e){
			e.preventDefault();
			$(this).parent().toggleClass('open').next().slideToggle();
		});
	}
};
/* jQuery for index*/
INS.Home = {
	init: function(){
		this.owlSliderHome();
		this.renderTopProduct();
		this.slideBlockHome();
		this.actionMediaScreen();
		this.loadImgElement();
	},
	owlSliderHome: function(){
		var sliderOWl = jQuery('.sliderWrap').owlCarousel({
			items: 1,
			lazyLoad:true,
			loop: false,
			autoplay: false,
			margin: 0,
			responsiveClass: true,
			paginationSpeed : 800,
			nav : false,
			navText: ['‹' , '›'],
			afterAction: function(){

			}
		});
		var size = $('.subWrap ul.listSub li').length,
				sizeWidth = 100 / size;
		$('.subWrap ul.listSub li').css('width',sizeWidth +'%');
		$('.subWrap ul.listSub li a').on('click', function(){
			$('.subWrap ul.listSub li').removeClass('active');
			$(this).parent().addClass('active');
			var index = $(this).parent().index();
			sliderOWl.trigger('to.owl.carousel', index)
		})
		sliderOWl.on('initialized.owl.carousel changed.owl.carousel refreshed.owl.carousel', function (event) {
			if (!event.namespace) return;
			var carousel = event.relatedTarget,
					element = event.target,
					current = carousel.current();
			$('.subWrap ul.listSub li').removeClass('active');
			$('.subWrap ul.listSub li').eq(current).addClass('active');
		})
	},
	renderTopProduct: function(){
		if($('#topProducts').size() > 0 ){
			var checkAuto = parseInt($('#topProducts').attr('data-auto'));
			if( checkAuto == 1 ){
				var str = '/collections/all?sort_by=best-selling&view=top';
				$.ajax({
					url: str,
					async:false,
					beforeSend: function() {},
					success: function(data) {
						var parsed = $.parseHTML(data);
						var html = $(parsed).filter('.itemTop[data-show="true"]').clone();
						$('#pdTopLisstting').append(html);
					}
				})
				var owl = $('#pdTopLisstting');
				setTimeout(function(){
					owl.on('initialize.owl.carousel initialized.owl.carousel ' +
								 'initialize.owl.carousel initialize.owl.carousel ' +
								 'resize.owl.carousel resized.owl.carousel ' +
								 'refresh.owl.carousel refreshed.owl.carousel ' +
								 'update.owl.carousel updated.owl.carousel ' +
								 'drag.owl.carousel dragged.owl.carousel ' +
								 'translate.owl.carousel translated.owl.carousel ' +
								 'to.owl.carousel changed.owl.carousel', function(e) {
									 $('#topProducts').fadeIn(500);
								 })
					owl.owlCarousel({
						items: 6,
						loop: false,
						autoplay: false,
						margin: 15,
						responsiveClass: true,
						dots: false,
						nav : true,
						navText: ['‹' , '›'],
						responsive: {
							0: {
								items: 1
							},
							320: {
								items: 2
							},
							480: {
								items: 2
							},
							767: {
								items: 3
							},
							992: {
								items: 5
							},
							1200: {
								items: 6
							}
						}
					});
					owl.find('.owl-prev').addClass('disabled')
					owl.on('initialized.owl.carousel changed.owl.carousel refreshed.owl.carousel', function (event) {
						if (!event.namespace) return;
						var carousel = event.relatedTarget,
								element = event.target,
								current = carousel.current();
						$('.owl-next', element).toggleClass('disabled', current === carousel.maximum());
						$('.owl-prev', element).toggleClass('disabled', current === carousel.minimum());
					})
					INS.actionResize.init();
				},1000)
			}else{
				var owl = $('#pdTopLisstting');
				owl.on('initialize.owl.carousel initialized.owl.carousel ' +
							 'initialize.owl.carousel initialize.owl.carousel ' +
							 'resize.owl.carousel resized.owl.carousel ' +
							 'refresh.owl.carousel refreshed.owl.carousel ' +
							 'update.owl.carousel updated.owl.carousel ' +
							 'drag.owl.carousel dragged.owl.carousel ' +
							 'translate.owl.carousel translated.owl.carousel ' +
							 'to.owl.carousel changed.owl.carousel', function(e) {
								 $('#topProducts').fadeIn(500);
							 })
				owl.owlCarousel({
					items: 6,
					loop: false,
					autoplay: false,
					margin: 15,
					responsiveClass: true,
					dots: false,
					nav : true,
					navText: ['‹' , '›'],
					responsive: {
						0: {
							items: 1
						},
						320: {
							items: 2
						},
						480: {
							items: 2
						},
						767: {
							items: 3
						},
						992: {
							items: 5
						},
						1200: {
							items: 6
						}
					}
				});
				owl.find('.owl-prev').addClass('disabled')
				owl.on('initialized.owl.carousel changed.owl.carousel refreshed.owl.carousel', function (event) {
					if (!event.namespace) return;
					var carousel = event.relatedTarget,
							element = event.target,
							current = carousel.current();
					$('.owl-next', element).toggleClass('disabled', current === carousel.maximum());
					$('.owl-prev', element).toggleClass('disabled', current === carousel.minimum());
				})
			}
		}
	},
	slideBlockHome: function(){
		$('.slidePDHome').each(function(){
			var sizeChild = $(this).children('.productItem').size();
			if(sizeChild > 0 ){
				var owl = $(this);
				owl.parent().hide();
				owl.on('initialize.owl.carousel initialized.owl.carousel ' +
							 'initialize.owl.carousel initialize.owl.carousel ' +
							 'resize.owl.carousel resized.owl.carousel ' +
							 'refresh.owl.carousel refreshed.owl.carousel ' +
							 'update.owl.carousel updated.owl.carousel ' +
							 'drag.owl.carousel dragged.owl.carousel ' +
							 'translate.owl.carousel translated.owl.carousel ' +
							 'to.owl.carousel changed.owl.carousel', function(e) {
								 owl.parent().fadeIn(500);
							 })
				owl.owlCarousel({
					items: 4,
					slideBy: 4,
					loop: false,
					autoplay: false,
					margin: 15,
					responsiveClass: true,
					dots: false,
					nav : true,
					navText: ['‹' , '›'],
					responsive: {
						0: {
							items: 1
						},
						320: {
							items: 2
						},
						480: {
							items: 2
						},
						767: {
							items: 3
						},
						992: {
							items: 4
						},
						1200: {
							items: 4
						}
					}
				});
				owl.find('.owl-prev').addClass('disabled')
				owl.on('initialized.owl.carousel changed.owl.carousel refreshed.owl.carousel', function (event) {
					if (!event.namespace) return;
					var carousel = event.relatedTarget,
							element = event.target,
							current = carousel.current();
					$('.owl-next', element).toggleClass('disabled', current === carousel.maximum());
					$('.owl-prev', element).toggleClass('disabled', current === carousel.minimum());
				})
			}
		})
		if($('.slideBlogHome').children().size() > 0 ){
			var owl = $('.slideBlogHome');
			owl.owlCarousel({
				items: 3,
				loop: false,
				autoplay: false,
				margin: 15,
				responsiveClass: true,
				dots: false,
				nav : true,
				navText: ['‹' , '›'],
				responsive: {
					0: {
						items: 1
					},
					320: {
						items: 1
					},
					480: {
						items: 1
					},
					767: {
						items: 2
					},
					992: {
						items: 3
					},
					1200: {
						items: 3
					}
				}
			});
			owl.find('.owl-prev').addClass('disabled')
			owl.on('initialized.owl.carousel changed.owl.carousel refreshed.owl.carousel', function (event) {
				if (!event.namespace) return;
				var carousel = event.relatedTarget,
						element = event.target,
						current = carousel.current();
				$('.owl-next', element).toggleClass('disabled', current === carousel.maximum());
				$('.owl-prev', element).toggleClass('disabled', current === carousel.minimum());
			})
		}
	},
	actionMediaScreen: function(){
		$(document).on('click','.openMenuTabs a', function(){
			$(this).parents('.blockTitle').toggleClass('open');
		})
	},
	lazyLoadImg: function(){
		var $imgLazy = $('img.imgLazy');
		if($imgLazy.length == 0 )
			return;
		$.each($imgLazy, function(index,img){
			var src = $(img).data('src');
			$(img).attr('src',src).removeAttr('data-src');
		})
	},
	loadImgElement: function(){
		var $this = this;
		setTimeout(function(){
			$this.lazyLoadImg();
		},0)
		$(window).scroll(function(){
			$this.lazyLoadImg();
		})
	}
};
/* js load page */
INS.loadPage = {
	init : function(){
		this.pageLoad();
		if(window.shop.template == 'index' ){
			this.responsiveHeading();
		}
	},
	pageLoad: function(){
		$('.preloader').delay(1000).fadeOut(500);
		//setTimeout(function(){$(window).trigger('resize');},4000)
	},
	responsiveHeading: function(){
		if($(window).width() < 992 ){
			$('.blockSection .blockTitle').removeClass('responHeading');
		}else{
			var $blockSeasion = $('.blockSection');
			$.each($blockSeasion, function(index,el){
				var headWidth = $(el).find('.blockTitle').width(),
						h2Width = $(el).find('.blockTitle h2').width(),
						compWidth = headWidth - h2Width - 20,
						ulWidth = $(el).find('.navBlock').width();
				if( ulWidth >= compWidth ){
					$(el).find('.blockTitle').addClass('responHeading');
				}else{
					$(el).find('.blockTitle').removeClass('responHeading');
				}
			})
		}
	}
}
/* js Product page */
INS.Product = {
	init: function(){
		this.ajaxAddCart();
	},
	ajaxAddCart: function(){
		$(document).on('click','.btn-addCart', function(){
			var qty = parseInt($('.wrapBlockInfo .groupQty input').val()),
					variantID = $('#product-select').val(),
					cart = ($('.headerTemp').hasClass('headerFixed')) ? $('.tempFixed .itemFixed.cartFixed') : $('#headerUser .wrap .cartBox .icon'),
					image = $('.wrapperPdImage .featureImg img');
			cart.addClass('loading');
			INS.Main.flyToElement(image,cart);
			INS.Main.ajaxAddCart(qty,variantID);
		})
	}
}
/* js Collection page */
INS.Collection = {
	init: function(){
		this.toggleFilter();
		this.ajaxClickInputFilter();
		this.toggleNavMobile();
	},
	toggleFilter: function(){
		$(document).on('click', '.toggleFilter a, .overlayFilter', function(){
			$('body').toggleClass('openFilter');
		})
	},
	ajaxClickInputFilter: function(){
		$(document).on('click','.check-box-list li > input',function(){
			jQuery(this).parent().toggleClass('active');
			var _url = INS.Collection.ajaxGetFilterQuery() + '&view=filter&page=1';
			INS.Collection.ajaxRenderHTML(_url);
		})
		$(document).on('click','.content_sortPagiBar.pagiFilter li a', function(e){
			e.preventDefault();
			INS.Collection.ajaxRenderHTML($(this).attr('href'))
		});
	},
	ajaxGetFilterQuery: function(){
		var _query = '', _price = '', _vendor = '', _color = '', _size = '', _id = '';
		_id = $('#coll-handle').val();
		var _str = '/search?q=filter=';
		_query = "("+_id+")";

		jQuery('.filter-price ul.check-box-list li.active').each(function(){
			_price = _price + jQuery(this).find('input').data('price') + '||';
		})
		_price=_price.substring(0,_price.length -2);
		if(_price != ""){
			_price='('+_price+')';
			_query+='&&'+_price;
		}

		jQuery('.filter-brand ul.check-box-list li.active').each(function(){
			_vendor = _vendor + jQuery(this).find('input').data('vendor') + '||';
		})
		_vendor=_vendor.substring(0,_vendor.length -2);
		if(_vendor != ""){
			_vendor='('+_vendor+')';
			_query+='&&'+_vendor;
		}

		jQuery('.filter-color ul.check-box-list li.active').each(function(){
			_color = _color + jQuery(this).find('input').data('color') + '||';
		})
		_color=_color.substring(0,_color.length -2);
		if(_color != ""){
			_color='('+_color+')';
			_query+='&&'+_color;
		}

		jQuery('.filter-size ul.check-box-list li.active').each(function(){
			_size = _size + jQuery(this).find('input').data('size') + '||';
		})
		_size=_size.substring(0,_size.length -2);
		if(_size != ""){
			_size='('+_size+')';
			_query+='&&'+_size;
		}
		_str += encodeURIComponent(_query);
		return _str;
	},
	ajaxRenderHTML: function(str){
		jQuery.ajax({
			url : str,
			async: false,
			success: function(data){
				jQuery("#pd_collection").html(data);
			}
		});
		if(sessionStorage.page_view == 'view_grid' ){
			$('.filter').removeClass('view_list').addClass('view_grid');
		}else{
			$('.filter').removeClass('view_grid').addClass('view_list');
		}
		INS.actionResize.init();
		INS.Main.replaceImageLoop();
		INS.Main.scrollReplaceRegExpImg();
	},
	toggleNavMobile: function(){
		$(document).on('click','#childNavCatelog .menu-item.head a', function(){
			$(this).parents('#childNavCatelog').fadeOut(500);
		})
	}
}
/* resize */
INS.actionResize = {
	init: function(){
		this.fixheightItem();
		if(window.shop.template == 'product' ){
			this.fixheightThumbProduct();
		}
	},
	fixheightItem: function(){
		$('.elementFixHeight').css('height','auto');
		setTimeout(function(){
			$('.blockFixHeight .elementFixHeight').each(function(i,el){
				var elWidth = $(el).width();
				$(el).height(elWidth);
			})
		},500)
	},
	fixheightThumbProduct: function(){
		var $el = $('.pdImgThumbs');
		if($el.size() > 0 ){
			$el.css('height','auto').removeClass('autoHeight');
			setTimeout(function(){
				var elHeight = $el.height(),
						feaHeight = $('#imgFeatured').height() + 70;
				if(elHeight > feaHeight){
					$el.css('height',$('#imgFeatured').height()).addClass('autoHeight');
				}
			},500)
		}
	},
}