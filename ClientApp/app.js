import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import lodash from 'lodash';  
import Vuetify from 'vuetify'
import Lightbox from 'vue-my-photos'
import Vuelidate from 'vuelidate'
import AsyncComputed from 'vue-async-computed'
import UUID from 'vue-uuid'
import PerfectScrollbar from 'vue2-perfect-scrollbar'

import 'material-design-icons-iconfont/dist/material-design-icons.css'
import 'vuetify/dist/vuetify.min.css'
import 'vue2-perfect-scrollbar/dist/vue2-perfect-scrollbar.css'

Vue.component('lightbox', Lightbox)

Vue.use(AsyncComputed)
Vue.config.devtools = true
Vue.config.performance = true
Vue.use(Lightbox)
Vue.use(Vuelidate)
Vue.use(UUID)
Vue.use(PerfectScrollbar)


const opts =  {
  icons: {
    iconfont: 'md',
  },
  theme:
  {
    dark: true,
    themes: {
      dark: {
        primary: '#fc0'
      }
    }
  }
}

Vue.use(Vuetify)
Vue.prototype.$http = axios;
Vue.prototype._ = lodash

sync(store, router);

const app = new Vue({  
  vuetify: new Vuetify(opts),
  store,
  router,
  ...App
});

export {
  app,
  router,
  store
}
