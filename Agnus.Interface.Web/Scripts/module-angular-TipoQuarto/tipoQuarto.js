//#define aplicação angular e o controller
var app = angular.module("agnusApp", []);

//#registra o controller e cria a função para obter os dados do Controlador MVC
app.controller("tipoQuartoCtrl", function ($scope, $http) {

    $http.get('/TipoQuarto/Listar/')
    .success(function (result) {
        $scope.listaTipoQuarto = result;
    })
    .error(function (data) {
        alert("Erro Processamento")
    });

    //chama o  método Salvar do controlador
    $scope.Salvar = function () {
        waitingDialog.show('Atualizando dados com a base, aguarde...'); 
        $http({
            method: 'POST',
            url: '/TipoQuarto/Salvar/',
            data: $scope.atipoQuarto,
        }).success(function (data, status, headers, config) {
            $scope.listaTipoQuarto = data
            delete $scope.atipoQuarto;
            setTimeout(function () { waitingDialog.hide(); ShowSucessDialogModal.show('Suas informações foram processadas em nossa base de dados com êxido.'); }, 2000);

        }).error(function (data, status, headers, config) {                      
            waitingDialog.hide();
            ShowErrorDialogModal.show(data.Error);
        });
    };

    //chama o  método Excluir do controlador
    $scope.Excluir = function (Id) {
        ShowDeleteDialogModal.show('Deseja Excluir o registro selecionado?');
        
        $('#excluir').click(function () {
            waitingDialog.show('Atualizando dados com a base, aguarde...');
            $http({
                method: 'POST',
                url: '/TipoQuarto/Excluir/',
                data: { Id: Id },
            }).success(function (data, status, headers, config) {
                if (data.Error != null) {
                    setTimeout(function () { waitingDialog.hide(); ShowErrorDialogModal.show(data.Error); }, 2000);
                } else {
                    $scope.listaTipoQuarto = data
                    setTimeout(function () { waitingDialog.hide(); ShowSucessDialogModal.show('Suas informações foram processadas em nossa base de dados com êxido.'); }, 2000);
                }

            }).error(function (data, status, headers, config) {
                waitingDialog.hide();
                ShowErrorDialogModal.show(status);
            });
        });
        
    };

});

$(document).ready(function () {
    $("#QuantidadePermitidaPessoas").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
});
