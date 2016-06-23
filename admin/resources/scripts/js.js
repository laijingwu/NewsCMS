window.onload = function(){
            var oExit = document.getElementById("exit");
    		var oShow = document.getElementById("video_show");
    		var oAdd = document.getElementById("add");
    		var oAddMain = document.getElementById("addMain");
    		oExit.onclick = function(){
    			oShow.style.display = "none";
    		}
    		oAdd.onclick = function(){
    			oShow.style.display = "block";
    		}
    		oAddMain.onclick = function () {
    		    oShow.style.display = "block";
    		}
    	}