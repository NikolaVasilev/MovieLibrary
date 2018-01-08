function show(input) {
    if (input.files && input.files[0]) {

        var canvas = document.getElementById("user_img_c");
        var ctx = canvas.getContext("2d");
        var img = new Image();

       

        img.onload = function () {

            // set size proportional to image
            //canvas.height = canvas.width * (img.height / img.width);

            // step 1 - resize to fill canvas
            var oc = document.createElement('canvas'),
                octx = oc.getContext('2d');

            var image_ratio = img.width / img.height;

            // canvas_ratio is 1:1 !!!
            if (image_ratio > 1) {
                oc.width = img.width / (img.height / canvas.height);
                oc.height = canvas.height;
            } else if (image_ratio < 1) {
                oc.width = canvas.width;
                oc.height = img.height / (img.width / canvas.width);
            } else {
                oc.width = canvas.width;
                oc.height = canvas.height;
            }

            octx.drawImage(img, 0, 0, oc.width, oc.height);

            // step 2, crop to final size
            ctx.drawImage(oc,
                0, 0, canvas.width, canvas.height,
                0, 0, canvas.width, canvas.height);
        }
        
        console.log(img);
        var filerdr = new FileReader();
        filerdr.onload = function (e) {
            if (e.target.result == '' || e.target.result == null) {
                img.src = "http://newhorizonindia.edu/nhengineering/wp-content/uploads/2016/07/member_default_img-200x200.png";
            } else {
                img.src = e.target.result;
            }
            
            //$('#user_img').attr('src', e.target.result);
        }
        filerdr.readAsDataURL(input.files[0]);
       
    }
}

