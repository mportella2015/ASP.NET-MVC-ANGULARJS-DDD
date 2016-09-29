/**
 * Módulo para a exibição de "Tela de Mensagens e Processamento..." diálogo utilizando Bootstrap
 */

var ShowDeleteDialogModal = ShowDeleteDialogModal || (function ($) {
    'use strict';

    // Criando diálogo modal
    var $dialog = $(
		'<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header" style="background-color:#F65327;color:white"><h4 style="margin:0;">Aviso do Sistema</h4></div>' +
			'<div class="modal-body" >' +
				'<h5 style="margin:0;"></h5>' +
			'</div>' +
           '  <div class="modal-footer">' +
           '     <div class="col-md-2"> ' +
           '        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cancelar</button>' +
           '     </div>' +
           '     <div class="col-md-2"> ' +
           '        <button id="excluir" type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Excluir</button>' +
           '     </div>' +
           '  </div>' +
		'</div></div></div>');

    return {
        /**
        * Abre o nosso diálogo
        * @ Param mensagem de mensagem personalizada
        * Opções @param opções feitas sob encomenda:
        * Options.dialogSize - inicialização postfix para o tamanho de diálogo, por exemplo, "SM", "m";
        * Options.progressType - inicialização postfix para o progresso tipo bar, por exemplo, "Sucesso", "aviso".
        */
        show: function (message, options) {
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'underfined-message';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // Este callback é executado após o diálogo estava escondida
            }, options);

            // Diálogo Configurando
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h5').text(message);
            // Adicionando callbacks
            if (typeof settings.onHide === 'function') {
                $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($dialog);
                });
            }
            // Abrindo a caixa de Diálogo 
            $dialog.modal();
        },

        hide: function () {
            $dialog.modal('hide');
        }
    };

})(jQuery);

var ShowSucessDialogModal = ShowSucessDialogModal || (function ($) {
    'use strict';

    // Criando diálogo modal
    var $dialog = $(
		'<div class="modal fade"  tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header" style="background-color:#01A2DE;color:white"><h4 style="margin:0;">Aviso do Sistema</h4></div>' +
			'<div class="modal-body" >' +
				'<h5 style="margin:0;"></h5>' +
			'</div>' +
           '  <div class="modal-footer">' +
           '     <div class="col-md-12"> ' +
           '        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Fechar</button>' +
           '     </div>'+
           '  </div>' +
		'</div></div></div>');

    return {
        /**
        * Abre o nosso diálogo
        * @ Param mensagem de mensagem personalizada
        * Opções @param opções feitas sob encomenda:
        * Options.dialogSize - inicialização postfix para o tamanho de diálogo, por exemplo, "SM", "m";
        * Options.progressType - inicialização postfix para o progresso tipo bar, por exemplo, "Sucesso", "aviso".
        */
        show: function (message, options) {
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'underfined-message';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // Este callback é executado após o diálogo estava escondida
            }, options);

            // Diálogo Configurando
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h5').text(message);
            // Adicionando callbacks
            if (typeof settings.onHide === 'function') {
                $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($dialog);
                });
            }
            // Abrindo a caixa de Diálogo 
            $dialog.modal();
        },

        hide: function () {
            $dialog.modal('hide');
        }
    };

})(jQuery);

var ShowErrorDialogModal = ShowErrorDialogModal || (function ($) {
    'use strict';

    // Criando diálogo modal
    var $dialog = $(
         '<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
         '<div class="modal-dialog modal-m">' +
         '<div class="modal-content">' +
             '<div class="modal-header" style="background-color:#F65327;color:white"><h4 style="margin:0;">Falha na operação do Sistema</h4></div>' +
             '<div class="modal-body" >' +
                 '<h5 style="margin:0;"></h5>' +
             '</div>' +
            '  <div class="modal-footer">' +
            '     <div class="col-md-2"> ' +
            '        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Fechar</button>' +
            '     </div>' +
            '  </div>' +
         '</div></div></div>');

    return {
        /**
        * Abre o nosso diálogo
        * @ Param mensagem de mensagem personalizada
        * Opções @param opções feitas sob encomenda:
        * Options.dialogSize - inicialização postfix para o tamanho de diálogo, por exemplo, "SM", "m";
        * Options.progressType - inicialização postfix para o progresso tipo bar, por exemplo, "Sucesso", "aviso".
        */
        show: function (message, options) {
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'underfined-message';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // Este callback é executado após o diálogo estava escondida
            }, options);

            // Diálogo Configurando
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h5').text(message);
            // Adicionando callbacks
            if (typeof settings.onHide === 'function') {
                $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($dialog);
                });
            }
            // Abrindo a caixa de Diálogo 
            $dialog.modal();
        },

        hide: function () {
            $dialog.modal('hide');
        }
    };

})(jQuery);

var waitingDialog = waitingDialog || (function ($) {
    'use strict';

    // Criando diálogo modal
    var $dialog = $(
		'<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header" style="background-color:#01A2DE;color:white"><h3 style="margin:0;"></h3></div>' +
			'<div class="modal-body" >' +
				'<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
			'</div>' +
		'</div></div></div>');

    return {
        /**
        * Abre o nosso diálogo
        * @ Param mensagem de mensagem personalizada
        * Opções @param opções feitas sob encomenda:
        * Options.dialogSize - inicialização postfix para o tamanho de diálogo, por exemplo, "SM", "m";
        * Options.progressType - inicialização postfix para o progresso tipo bar, por exemplo, "Sucesso", "aviso".
        */
        show: function (message, options) {
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'Loading';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // Este callback é executado após o diálogo estava escondida
            }, options);

            // Diálogo Configurando
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h3').text(message);
            // Adicionando callbacks
            if (typeof settings.onHide === 'function') {
                $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($dialog);
                });
            }
            // Abrindo a caixa de Diálogo 
            $dialog.modal();
        },
        
        hide: function () {
            $dialog.modal('hide');
        }
    };

})(jQuery);
