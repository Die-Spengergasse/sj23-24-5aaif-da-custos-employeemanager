import{_ as f,o as d,c as i,a as c,w as l,u as n,b as p,F as _,r as C,p as g,d as v,e as w,t as m,f as V,g as y,h as b}from"./index-iAiioZAi.js";import{s as E,a as N,b as S}from"./column.esm-Gt_K9cmx.js";const k=s=>(g("data-v-3ff8f644"),s=s(),v(),s),B={class:"manageCustomersView"},I=k(()=>w("h1",null,"Kunden verwalten",-1)),L={key:0,class:"error"},M={data(){return{customers:null,validation:{},columns:[{field:"guid",header:"Guid",hidden:!0},{field:"name",header:"Name"},{field:"zip",header:"PLZ"},{field:"city",header:"Ort"},{field:"street",header:"Adresse"}]}},async mounted(){const s=await p.get("customers");this.customers=s.data},methods:{async onCellEditComplete(s){let{data:a,newValue:u,field:r}=s;try{a[r]=u,this.validation={},await p.put("customers",a)}catch(e){e.response?e.response.data.errors?e.response.status==400&&(this.validation=Object.keys(e.response.data.errors).reduce((t,o)=>{const h=o.charAt(0).toLowerCase()+o.slice(1);return t[h]=e.response.data.errors[o][0],t},{guid:a.guid})):alert(e.response.data):alert("Der Server ist nicht erreichbar.")}}}},O=Object.assign(M,{__name:"ManageCustomers",setup(s){return(a,u)=>(d(),i("div",B,[I,c(n(E),{value:a.customers,editMode:"cell",onCellEditComplete:a.onCellEditComplete},{default:l(()=>[(d(!0),i(_,null,C(a.columns,r=>(d(),b(n(N),{key:r.field,field:r.field,header:r.header,hidden:r.hidden,sortable:""},{body:l(({data:e,field:t})=>[V(m(e[t])+" ",1),a.validation.guid==e.guid?(d(),i("div",L,m(a.validation[t]),1)):y("",!0)]),editor:l(({data:e,field:t})=>[c(n(S),{modelValue:e[t],"onUpdate:modelValue":o=>e[t]=o,autofocus:""},null,8,["modelValue","onUpdate:modelValue"])]),_:2},1032,["field","header","hidden"]))),128))]),_:1},8,["value","onCellEditComplete"])]))}}),D=f(O,[["__scopeId","data-v-3ff8f644"]]);export{D as default};
