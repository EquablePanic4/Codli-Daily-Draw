let usersArr = (() => function () {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            return JSON.parse(this.responseText);
        }
    };

    xmlhttp.open("GET", "/users.json", true);
    xmlhttp.send();
});

let drawsArr = (() => function () {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            return JSON.parse(this.responseText);
        }
    };

    xmlhttp.open("GET", "/draws.json", true);
    xmlhttp.send();
});

function LogUsers() {
    console.log(usersArr);
}

function PerformCalculation() {

}