

var curPos,
	popPos,
	popActive = false,
	myClock = null;

var ua = navigator.userAgent;
var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(ua);


var IEMobile = ua.match(/IEMobile/i);
var isIE9 = /MSIE 9/i.test(ua);
var isIE10 = /MSIE 10/i.test(ua);
var isIE11 = /rv:11.0/i.test(ua) && !IEMobile ? true : false;

if (isIE9 || isIE10 || isIE11) {
	$('body').addClass('isIE');
}

function inputHolder() {
	$('.fs_error_txt').click(function () {
		$(this).parent().removeClass('fs-show-error');
		$(this).parent().find('input').focus();
	});
	$('input[type="text"]').focus(function (e) {
		$(this).parent().removeClass('fs-show-error');
	});

}

function ImgLazyLoad() {
    lazyImages = window.innerWidth > 1100 ? document.querySelectorAll('.cmPic.fs-lazy, .pcPic.fs-lazy, .active .cmPic.fs-lazyExtra, .active .pcPic.fs-lazyExtra') : document.querySelectorAll('.cmPic.fs-lazy, .spPic.fs-lazy, .active .cmPic.fs-lazyExtra, .active .spPic.fs-lazyExtra');
    [].slice.call(lazyImages).forEach(function (elm) {
        var src = elm.getAttribute('data-src');
        elm.setAttribute('src', src);
        elm.classList.remove('fs-lazy');
    });

    lazyBgs = window.innerWidth > 1100 ? document.querySelectorAll('.cmBg.fs-lazy, .pcBg.fs-lazy, .active .pcBg.fs-lazyExtra, .active .cmBg.fs-lazyExtra') : document.querySelectorAll('.cmBg.fs-lazy, .spBg.fs-lazy, .active .cmBg.fs-lazyExtra, .active .spBg.fs-lazyExtra');
    [].slice.call(lazyBgs).forEach(function (elm) {
        var src = elm.getAttribute('data-src');
        elm.style.backgroundImage = 'url(' + src + ')';
        elm.classList.remove('fs-lazy');
        elm.classList.remove('fs-lazyExtra');
    });

}

//SET LIMITED TIME FOR BOOT TICKET
function setClock(value) {

    var timeClock = value;

    myClock = $('.clock').FlipClock(timeClock, {
        clockFace: 'DailyCounter',
        countdown: true,
        autoStart: false,
        callbacks: {
            start: function () {
                $('.message').html('The clock has started!');
            }
        }
    });
}
function fsEvent() {

	//Select list
	$('.fs-select-header').click(function (e) {

		var box = $(this).parent();
		if (box.hasClass('open')) {
			box.removeClass('open');
		} else {
			$('.fs-select-list').removeClass('open');
			box.addClass('open');
		}

	});

	//$('.fs-select-box li').click(function () {
	//	var that = $(this);
	//	var box = $(this).parent().parent().parent();

	//	if (!that.hasClass('selected')) {
	//		box.find('li').removeClass('selected');
	//		that.addClass('selected');
	//		box.removeClass('open');

	//		target = that.data('value');
	//		if (box.hasClass('fsSelectPromotion')) {
	//			$('.is-single img').removeClass('active');
	//			$('.is-single img[data-target=' + target + ']').addClass('active');
	//		} else {
	//			box.find('.fs-select-header h3').html(that.text());
	//		}
	//	}

	//});

	$(document).on('click touchstart', function (event) {
		if ($(".fs-select-list").has(event.target).length == 0 && !$(".fs-select-list").is(event.target)) {
			$(".fs-select-list").removeClass("open");
		}
	});
	//End

	//Close poppup
	$('.overlay-form, .close-overlay').click(function (e) {
		e.preventDefault();
		popActive = false;
		$('.overlay-form').removeClass('active');
		$('body').removeClass('no-scroll');
		$('html,body').scrollTop(popPos);
	});

	//if ($('.clock').length) {
	//	setClock();
	//	myClock.start();
	//}


	////Temp code show thank pop
	//$('#fs_hunter_form .fs-btn').click(function () {
	//	$('.fs-confirm').fadeIn(0);
	//	$('.is-form, .fs-payment').addClass('active');
	//	ImgLazyLoad();
	//	var delTop = 80;
	//	if (window.innerWidth <= 1100) {
	//		delTop = 118;
	//	}
	//	var top = $('.is-form').offset().top - delTop;
	//	$('html,body').animate({ scrollTop: top }, 500);
	//});
	//$('.close-payment, .btn-confirm-next').click(function () {
	//	$('.is-form, .fs-payment').removeClass('active');
	//	setTimeout(function () {
	//		$('.fs-payment-box').css({ 'display': 'none' });
	//	}, 300);
	//});

	//$('.btn-confirm').click(function () {
	//	$('.fs-confirm').fadeOut(300, function () {
	//		$('.fs-confirm').removeClass('fs-show');
	//		$('.fs-success').addClass('fs-show');
	//		$('.fs-success').fadeIn(0);
	//	});
	//});

	//Close overlay
	$('.fs-close-overlay').click(function () {
		$('.fs-overlay').removeClass('active');
		$('body').removeClass('fs-no-scroll');
	});

	//Nav click
	$('.fs-navigation li').click(function () {
		var target = $(this).attr('data-target');
		if ($('.' + target).length) {

			if (window.innerWidth > 1100) {
				if ($('.fs-overlay.active').length) {

					setTimeout(function () {
						$('body').removeClass('fs-no-scroll');
						$('.fs-overlay').removeClass('active');
						var top = $('.' + target).offset().top - 80;
						$('html,body').animate({ scrollTop: top }, 500);
					}, 150);

				} else {
					var top = $('.' + target).offset().top - 80;
					$('html,body').animate({ scrollTop: top }, 500);
				}
			} else {

				setTimeout(function () {
					$('.fs-overlay, .fs-nav-but, .fs-navigation').removeClass('active');
					$('body').removeClass('fs-no-scroll');
					var top = $('.' + target).offset().top - 118;
					$('html,body').animate({ scrollTop: top }, 500);
				}, 150);
			}

		}
	});


	//Radio hide
	$('.fs-form-radio .fs-radio-input').click(function () {
		var target = $(this).attr('data-target');
		var $fsCurBox = $('.fs-flow-radio.active');

		if (!$('.fs-flow-radio[data-target=' + target + ']').hasClass('active')) {
			$fsCurBox.fadeOut(300, function () {
				$fsCurBox.removeClass('active');
				$('.fs-flow-radio[data-target=' + target + ']').addClass('active');
				$('.fs-flow-radio[data-target=' + target + ']').fadeIn(0);
			});
		}

	});

	//Nav but
	$('.fs-nav-but').click(function () {
		if ($(this).hasClass('active')) {
			$('.fs-nav-but, .fs-navigation').removeClass('active');
			$('body').removeClass('fs-no-scroll');
		} else {
			$('.fs-nav-but, .fs-navigation').addClass('active');
			$('body').addClass('fs-no-scroll');
		}
	});

	//Goto form
    $('.fs-overlay-inr .fs-btn,.gotoForm').click(function () {
		setTimeout(function () {
			$('.fs-overlay').removeClass('active');
			$('body').removeClass('fs-no-scroll');
			var delTop = 80;
			if (window.innerWidth <= 1100) {
				delTop = 118;
			}
			var top = $('.is-form').offset().top - delTop;
			$('html,body').animate({ scrollTop: top }, 500);
		}, 150);
	});

    $('.fs-header-inr .fs-btn, .is-supper .fs-btn, .is-timeline .fs-btn').click(function () {
        var delTop = 80;
        if (window.innerWidth <= 1100) {
            delTop = 118;
        }
        var top = $('.is-form').offset().top - delTop;
        $('html,body').animate({ scrollTop: top }, 500);
    });


	//Temp Open Game
	$('.is-star .fs-btn,  .is-lost .btn-play-again, .is-win .btn-play-again, .is-not-finish .btn-continute').click(function () {
		indexGame = 0;
		resultTrue = 0;
        $('.fs-question-nav li').removeClass('active');
        $('.fs-question-nav ul li').html('');
		$('.fs-game-pop').removeClass('active');
		$('.fs-game-player').addClass('active');
		///$('#fs-game-wrapper').html('');

        setTimeout(function () {
            ResetGame();
			InitGame(indexGame);
		}, 150);

	});

	//$('.win-open-first .fs-area-pic').click(function () {
	//	$('.fs-gilt-box .fs-box').css({ 'display': 'none' });
	//	$('.win-open-winner').fadeIn(300);
	//});

    /*Condtion Pop*/
    $('.fs-condition-link').click(function () {
        $('body').addClass('fs-no-scroll');
        $('.fs-overlay-condition').addClass('active');
    });
    $('.fs-close-overlay-condition').click(function () {
        $('body').removeClass('fs-no-scroll');
        $('.fs-overlay-condition').removeClass('active');
    });
	inputHolder();

}

function sfSlider() {

	
	if ($('.fs-timeline-slider').length) {
		fsSlider = new Swiper('.fs-timeline-slider', {
			simulateTouch: true,
			loop: true,
			speed: 500,
			navigation: {
				nextEl: '.is-timeline .fs-nav-next',
				prevEl: '.is-timeline .fs-nav-prev',
			},
			pagination: {
				el: '.is-timeline .pagination',
				clickable: true
			},
			on: {
				init: function () {

				},
				transitionStart: function () {
				},
				transitionEnd: function () {

				}
			}
		});
	}


}


//Game

var Question = [];
var QuestionOriginal = [{
    Id:1,
	Title: "Màn hình của Samsung Galaxy Note10 + tại Việt Nam<br>có kích thước là bao nhiêu?",
	Answer: [{
		Content: "<p>5.8 <span>Inch</span></p>",
		Url: '../../images/answer-bg-01.png',
		IsRight: false
	}, {
		Content: "<p>6.1 <span>inch</span></p>",
		Url: '../../images/answer-bg-03.png',
		IsRight: false
	}, {
		Content: "<p>6.8 <span>Inch</span></p>",
		Url: '../../images/answer-bg-06.png',
		IsRight: true
	}, {
		Content: "<p>6.2 <span>Inch</span></p>",
		Url: '../../images/answer-bg-02.png',
		IsRight: false
	}, {
		Content: "<p>5.8 <span>Inch</span></p>",
		Url: '../../images/answer-bg-04.png',
		IsRight: false
	}, {
		Content: "<p>9.8 <span>Inch</span></p>",
		Url: '../../images/answer-bg-05.png',
		IsRight: false
	}, {
		Content: "<p>6.8 <span>Inch</span></p>",
		Url: '../../images/answer-bg-06.png',
		IsRight: true
	}, {
		Content: "<p>18 <span>Inch</span></p>",
		Url: '../../images/answer-bg-07.png',
		IsRight: false
	}, {
		Content: "<p>8 <span>Inch</span></p>",
		Url: '../../images/answer-bg.png',
		IsRight: false
	}, {
		Content: "<p>8.1 <span>Inch</span></p>",
		Url: '../../images/answer-bg-08.png',
		IsRight: false
	}, {
		Content: "<p>9.8 <span>Inch</span></p>",
		Url: '../../images/answer-bg-09.png',
		IsRight: false
	}]
},
    {
    Id:2,
	Title: "Dung lượng pin của Note10+ tại Việt Nam là bao nhiêu?",
	Answer: [{
		Content: "<p>4000 <span>mAh</span></p>",
		Url: '../../images/answer-bg-01.png',
		IsRight: false
	}, {
		Content: "<p>3700 <span>mAh</span></p>",
		Url: '../../images/answer-bg-03.png',
		IsRight: false
	}, {
		Content: "<p>4700 <span>mAh</span></p>",
		Url: '../../images/answer-bg-02.png',
		IsRight: false
	}, {
		Content: "<p>4300 <span>mAh</span></p>",
		Url: '../../images/answer-bg-04.png',
		IsRight: true
	}, {
		Content: "<p>3300 <span>mAh</span></p>",
		Url: '../../images/answer-bg-08.png',
		IsRight: false
	}, {
		Content: "<p>3200 <span>mAh</span></p>",
		Url: '../../images/answer-bg-05.png',
		IsRight: false
	}, {
		Content: "<p>2000 <span>mAh</span></p>",
		Url: '../../images/answer-bg-07.png',
		IsRight: false
	}, {
		Content: "<p>3700 <span>mAh</span></p>",
		Url: '../../images/answer-bg-06.png',
		IsRight: false
	}, {
		Content: "<p>1000 <span>mAh</span></p>",
		Url: '../../images/answer-bg-09.png',
		IsRight: false
	}]
},
    {
    Id:3,
	Title: "RAM của Note10+ tại Việt Nam là bao nhiêu?",
	Answer: [{
		Content: "<p>10 <span>GB</span></p>",
		Url: '../../images/answer-bg-01.png',
		IsRight: false
	}, {
		Content: "<p>21 <span>GB</span></p>",
		Url: '../../images/answer-bg-04.png',
		IsRight: false
	}, {
		Content: "<p>16 <span>GB</span></p>",
		Url: '../../images/answer-bg-02.png',
		IsRight: false
	}, {
		Content: "<p>08 <span>GB</span></p>",
		Url: '../../images/answer-bg-08.png',
		IsRight: false
	}, {
		Content: "<p>22 <span>GB</span></p>",
		Url: '../../images/answer-bg-05.png',
		IsRight: false
	}, {
		Content: "<p>12 <span>GB</span></p>",
		Url: '../../images/answer-bg-03.png',
		IsRight: true
	}, {
		Content: "<p>10 <span>GB</span></p>",
		Url: '../../images/answer-bg-07.png',
		IsRight: false
	}, {
		Content: "<p>14 <span>GB</span></p>",
		Url: '../../images/answer-bg-09.png',
		IsRight: false
	}, {
		Content: "<p>15 <span>GB</span></p>",
		Url: '../../images/answer-bg-06.png',
		IsRight: false
	}]
},
    {
    Id:4,
	Title: "Note10+ tại Việt Nam có tổng cộng bao nhiêu màu?",
	Answer: [{
		Content: "<p>10</p>",
		Url: '../../images/answer-bg-01.png',
		IsRight: false
	}, {
		Content: "<p>7</p>",
		Url: '../../images/answer-bg-04.png',
		IsRight: false
	}, {
		Content: "<p>16</p>",
		Url: '../../images/answer-bg-02.png',
		IsRight: false
	}, {
		Content: "<p>4</p>",
		Url: '../../images/answer-bg-08.png',
		IsRight: false
	}, {
		Content: "<p>6</p>",
		Url: '../../images/answer-bg-05.png',
		IsRight: false
	}, {
		Content: "<p>3</p>",
		Url: '../../images/answer-bg-07.png',
		IsRight: true
	}, {
		Content: "<p>8</p>",
		Url: '../../images/answer-bg-03.png',
		IsRight: false
	}, {
		Content: "<p>2</p>",
		Url: '../../images/answer-bg-06.png',
		IsRight: false
	}, {
		Content: "<p>14</p>",
		Url: '../../images/answer-bg-09.png',
		IsRight: false
	}]
},
    {
    Id:5,
	Title: "Tên tính năng mới trên camera của Note10+ tại Việt Nam là gì?",
	Answer: [{
		Content: "<p>VOV</p>",
		Url: '../../images/answer-bg-07.png',
		IsRight: false
	}, {
		Content: "<p>KOF</p>",
		Url: '../../images/answer-bg-04.png',
		IsRight: false
	}, {
		Content: "<p>LOL</p>",
		Url: '../../images/answer-bg-02.png',
		IsRight: false
	}, {
		Content: "<p>VOF</p>",
		Url: '../../images/answer-bg-08.png',
		IsRight: false
	}, {
		Content: "<p>COF</p>",
		Url: '../../images/answer-bg-05.png',
		IsRight: false
	}, {
		Content: "<p>TOF</p>",
		Url: '../../images/answer-bg.png',
		IsRight: true
	}, {
		Content: "<p>IOS</p>",
		Url: '../../images/answer-bg-01.png',
		IsRight: false
	}, {
		Content: "<p>NOS</p>",
		Url: '../../images/answer-bg-06.png',
		IsRight: false
	}, {
		Content: "<p>LOF</p>",
		Url: '../../images/answer-bg-09.png',
		IsRight: false
	}]
},
];


var indexGame = 0;
var resultTrue = 0;
var isPlay = false;

var $player = $('#fs-game-player'),
	$wrapper = $('#fs-game-wrapper');

var leftMouse = 155,
	rightMouse = 75,
	leftSpaceRandoom = 120,
	rightSpaceRandoom = 30;

if (window.innerWidth <= 1100) {
	leftMouse = rightMouse = 0;
	leftSpaceRandoom = rightSpaceRandoom = 5;
}

function movePlayer(e) {
	if (e.pageX >= window.innerWidth - rightMouse) {
		e.pageX = window.innerWidth - rightMouse;
	} else if (e.pageX <= leftMouse) {
		e.pageX = leftMouse;
	}
	TweenLite.to($player, 1 / 10000, {
		css: {
			left: e.pageX
		}
	});
}

$($wrapper).on('mousemove', movePlayer);
$($wrapper).on('touchmove', movePlayer);

TweenLite.set("#fs-game-wrapper", { perspective: 600 });

var container = document.getElementById("fs-game-wrapper"),
	w = window.innerWidth,
	h = window.innerHeight;

function R(min, max) { return min + Math.random() * (max - min) };
var listQuestionIdOriginal = [1, 2, 3, 4, 5];
function ResetGame() {
    Question = [];
    // Refresh question list with random
    var listIdQuestion = listQuestionIdOriginal;
    for (var i = 0; i < 5; i++) {
        var max = listIdQuestion.length;
        var min = 0;        
        var randomItem = Math.floor(Math.random() * (max - min)) +min; 
        var idTemp = listIdQuestion[randomItem];
        var itemAdd = QuestionOriginal.find(x => x.Id == idTemp);
        Question.push(itemAdd);
        listIdQuestion = listIdQuestion.filter(function (value, index, arr) {

            return value !== idTemp;

        });
    }
}

function InitGame(index) {

	isPlay = true;
    if (Question[index] != null) {
        $('.fs-question-nav ul li:nth-child(' + (index + 1) + ')').addClass('active');
        $('.fs-question-title h3').html(Question[index].Title);

        for (i = 0; i < Question[index].Answer.length; i++) {
            var answer = Question[index].Answer[i];

            var ball = document.createElement('div');
            ball.innerHTML = answer.Content;
            ball.style.backgroundImage = 'url(' + answer.Url + ')';
            ball.setAttribute('data-right', answer.IsRight);

            TweenMax.set(ball, { attr: { class: 'fs-answer' }, x: R(leftSpaceRandoom, w - leftSpaceRandoom), y: -450 });

            container.appendChild(ball);
            animm(ball);
        }
    }
    else {
        console.log("AAAA");
    }
	

     function animm(elm) {
          TweenMax.to(elm, R(8, 40), { y: h + 50, ease: Linear.easeNone, repeat: -1, delay: -1 });

        TweenMax.to(elm, R(8, 40), { x: '+=5', repeat: -1, yoyo: true, ease: Sine.easeInOut });
 
       TweenMax.to(elm, R(8, 40), { repeat: -1, yoyo: true, ease: Sine.easeInOut, delay: -3, onUpdate: checkHit });
    }

	function checkHit() {
		var check = collision($(this.target));
		if (check) {
			if ($(this.target).attr('data-right') == 'true') {
				indexGame++;
				resultTrue++;
                if (resultTrue == 5) {//Win
                    $("#btnWinGame").click();
					isPlay = false;

				} else {
					TweenMax.killTweensOf('.fs-answer');
					$(".fs-answer").removeAttr("style");
					$('.fs-answer').remove();
                    $('.fs-question-nav ul li:nth-child(' + (index + 1) + ')').html($(this.target).html());
					InitGame(indexGame);
				}

			} else {
				resultTrue = 0;
				indexGame = 0;
				isPlay = false;

                TweenMax.killTweensOf('.fs-answer');
                $(".fs-answer").removeAttr("style");
				$('.fs-answer').remove();

                $('.fs-question-nav ul li').removeClass('active');
                $('.fs-question-nav ul li').html('');
				$('.fs-game-player').removeClass('active');
				$('.fs-game-box').css({ 'display': 'none' });
				$('.is-lost').fadeIn(0);
				$('.fs-game-pop').addClass('active');

			}
		}
	}

	function collision($ball) {

		var x1 = $ball.offset().left;
		var y1 = $ball.offset().top;

		var h1 = $ball.innerHeight();
		var w1 = $ball.innerWidth();

		var b1 = y1 + h1;
		var r1 = x1 + w1;

		var x2 = $player.offset().left;
        var y2 = $player.offset().top;

        var h2 = $player.innerHeight();
        var w2 = $player.innerWidth();

		var b2 = y2 + h2;
		var r2 = x2 + w2;
        if (b1 < y2 || y1 > b2 || r1 < x2 || x1 > r2) return false;
        //if (b1 < y2 + 50 || y1 > b2 + 50 || r1 < x2 + 40 || x1 > r2 + 40) return false;

		return true;
	}

}
//End Game

function onScroll() {
    curPos = $(window).scrollTop();


    [].slice.call(document.querySelectorAll('.is-game')).forEach(function (elm) {
        if (isPlay == true) {
            if (elm.getBoundingClientRect().top + $(window).height() < 150 || (elm.getBoundingClientRect().top + $(window).height()) / 2 > $(window).height()) {
                isPlay = false;
                $('.fs-question-nav li').removeClass('active');
                $('.fs-question-nav ul li').html('');
                $('.fs-game-pop').addClass('active');
                $('.fs-game-player').removeClass('active');
                $('.fs-game-box').css({ 'display': 'none' });
                $('.is-not-finish').fadeIn();
                TweenMax.killTweensOf('.fs-answer');
                $('.fs-answer').remove();
            }
        }
    });

}

function Rotate() {
    setTimeout(function () {
        $('.is-supper .fs-section-inr').css({ 'height': window.innerHeight - 118 });
        $('.is-game .fs-section-inr').css({ 'height': window.innerHeight - 50 });
    }, 100);
}

function Resize() {
    ImgLazyLoad();
}

$(window).on('scroll', onScroll);

$(window).on('resize', Resize);

$(window).on('load', function () {
	ImgLazyLoad();
	$('.fs-cnt-wrap').animate({ 'opacity': 1 }, 1000, function () {
		sfSlider();
		ImgLazyLoad();
	});
	fsEvent();
});

$(window).on("orientationchange", Rotate);

(function () {
	ImgLazyLoad();

	//$('body').addClass('fs-no-scroll');
	//$('.fs-overlay').addClass('active');

    if (window.innerWidth <= 1100) {
        $('.is-game .fs-section-inr').css({ 'height': window.innerHeight - 50 });
    }

})();